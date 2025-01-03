﻿#if NETFRAMEWORK
using System;
using System.IO;
using System.Reflection;

namespace Unicorn.Taf.Api
{
    /// <summary>
    /// Provides with ability to manipulate with Unicorn test assembly in dedicated <see cref="AppDomain"/>
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of worker on tests assembly</typeparam>
    [Serializable]
    public sealed class UnicornAppDomainIsolation<T> : IDisposable where T : MarshalByRefObject
    {
        private const string AppConfig = "app.config";

        [NonSerialized]
        private AppDomain _domain;
        private readonly string _assemblyDirectory;
        private readonly string _appDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnicornAppDomainIsolation{T}"/> class based on 
        /// specified base directory.
        /// </summary>
        /// <param name="baseDirectory">path to tests assembly directory</param>
        public UnicornAppDomainIsolation(string baseDirectory) : this(baseDirectory, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnicornAppDomainIsolation{T}"/> class based on 
        /// specified base directory and AppDomain name.
        /// </summary>
        /// <param name="baseDirectory">path to tests assembly directory</param>
        /// <param name="appDomainName">custom AppDomain name</param>
        public UnicornAppDomainIsolation(string baseDirectory, string appDomainName)
        {
            Type type = typeof(T);
            _assemblyDirectory = baseDirectory;
            _appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string appConfigFile = Path.Combine(baseDirectory, AppConfig);
            AppDomainSetup domainSetup = new AppDomainSetup()
            {
                ApplicationBase = baseDirectory,
                PrivateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath
            };

            if (File.Exists(appConfigFile))
            {
                domainSetup.SetConfigurationBytes(File.ReadAllBytes(appConfigFile));
            };

            _domain = AppDomain.CreateDomain(
                string.IsNullOrEmpty(appDomainName) ? "UnicornAppDomain:" + Guid.NewGuid() : appDomainName, 
                AppDomain.CurrentDomain.Evidence,
                domainSetup);

            _domain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            Instance = (T)_domain.CreateInstanceAndUnwrap(type.Assembly.Location, type.FullName);

            Environment.CurrentDirectory = _assemblyDirectory;
        }

        /// <summary>
        /// Gets assembly worker instance
        /// </summary>
        public T Instance { get; }

        /// <summary>
        /// Unloads current <see cref="AppDomain"/>
        /// </summary>
        public void Dispose()
        {
            if (_domain != null)
            {
                AppDomain.Unload(_domain);
                _domain = null;
            }
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyFile = GetAssemblyFile(args.Name);

            if (File.Exists(assemblyFile))
            {
                byte[] bytes = File.ReadAllBytes(assemblyFile);
                Assembly assessment = Assembly.Load(bytes);
                return assessment;
            }

            return Assembly.GetExecutingAssembly().FullName == args.Name ? Assembly.GetExecutingAssembly() : null;
        }

        private string GetAssemblyFile(string resolveEventArgName)
        {
            int index = resolveEventArgName.IndexOf(',');

            if (index < 0)
            {
                return resolveEventArgName;
            }

            string fileName = resolveEventArgName.Substring(0, index) + ".dll";
            string assemblyFile = Path.Combine(_assemblyDirectory, fileName);

            if (File.Exists(assemblyFile))
            {
                return assemblyFile;
            }

            assemblyFile = Path.Combine(_appDirectory, assemblyFile);
            if (File.Exists(assemblyFile))
            {
                return assemblyFile;
            }

            return assemblyFile;
        }
    }
}
#endif