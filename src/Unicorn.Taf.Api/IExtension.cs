using System;

namespace Unicorn.Taf.Api
{
    /// <summary>
    /// Represents interface for unicorn extensions.
    /// </summary>
    public interface IExtension : IDisposable
    {
        /// <summary>
        /// Executes extension code.
        /// </summary>
        void Execute(params object[] parameters);
    }
}
