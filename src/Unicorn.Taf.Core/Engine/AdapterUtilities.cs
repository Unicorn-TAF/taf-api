﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing.Tests;
using Unicorn.Taf.Core.Testing.Tests.Attributes;

namespace Unicorn.Taf.Core.Engine
{
    public static class AdapterUtilities
    {
        public static bool IsSuiteRunnable(Type suiteType)
        {
            var tags = from attribute
                           in suiteType.GetCustomAttributes(typeof(TagAttribute), true) as TagAttribute[]
                           select attribute.Tag.ToUpper().Trim();

            var name = (suiteType.GetCustomAttribute(typeof(SuiteAttribute), true) as SuiteAttribute)
                       .Name.ToUpper().Trim();

            if (!tags.Intersect(Config.RunTags).Any() && !Config.RunTags.Contains(name) && Config.RunTags.Any())
            {
                return false;
            }

            return suiteType.GetRuntimeMethods().Any(t => IsTestRunnable(t));
        }

        public static bool IsTestRunnable(MethodInfo testMethod)
        {
            if (testMethod.GetCustomAttribute(typeof(DisabledAttribute), true) != null)
            {
                return false;
            }

            var categories = from attribute
                                in testMethod.GetCustomAttributes(typeof(CategoryAttribute), true) as CategoryAttribute[]
                                select attribute.Category.ToUpper().Trim();
            
            var hasCategoriesToRun = categories.Intersect(Config.RunCategories).Count() == Config.RunCategories.Count;

            var fullTestName = GetFullTestMethodName(testMethod);
            var matchTestsMasks = !Config.RunTests.Any() || Config.RunTests.Any(m => Regex.IsMatch(fullTestName, m));
            return hasCategoriesToRun && matchTestsMasks;
        }

        public static bool IsSuiteParameterized(Type suiteType) =>
            suiteType.GetCustomAttribute(typeof(ParameterizedAttribute), true) != null;

        public static List<DataSet> GetSuiteData(Type suiteType)
        {
            var suiteDataMethod = suiteType.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(m => m.GetCustomAttribute(typeof(SuiteDataAttribute), true) != null);

            return suiteDataMethod == null ? new List<DataSet>() : suiteDataMethod.Invoke(null, null) as List<DataSet>;
        }

        public static bool IsTestParameterized(MethodInfo testMethod) =>
            testMethod.GetCustomAttribute(typeof(TestDataAttribute), true) != null;

        public static List<DataSet> GetTestData(string testDataMethod, object suiteInstance) =>
            suiteInstance.GetType().GetMethod(testDataMethod)
                .Invoke(suiteInstance, null) as List<DataSet>;

        public static string GetFullTestMethodName(MethodInfo testMethod) =>
            testMethod.ReflectedType.FullName + "." + testMethod.Name;
    }
}