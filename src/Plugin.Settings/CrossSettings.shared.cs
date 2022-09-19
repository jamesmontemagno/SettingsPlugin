﻿/*
 * Copyright (C) 2014 Refractored: 
 * 
 * Contributors:
 * http://github.com/JamesMontemagno
 * 
 * Original concept for Internal IoC came from: http://pclstorage.codeplex.com under Microsoft Public License (Ms-PL)
 * 
 */

using System;
using Plugin.Settings.Abstractions;

namespace Plugin.Settings
{
    /// <summary>
    /// Cross Platform settings
    /// </summary>
    public static class CrossSettings
    {
		static ISettings implementation;

		static ISettings Implementation => implementation ?? (implementation = CreateSettings());

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => Implementation == null ? false : true;

        /// <summary>
        /// Current plugin implementation to use
        /// </summary>
        public static ISettings Current
        {
            get
            {
                ISettings ret = Implementation;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
			set
			{
				implementation = value;
			}
        }

        static ISettings CreateSettings()
        {
#if NETSTANDARD 
            return null;
#else
#pragma warning disable IDE0022 // Use expression body for methods
            return new SettingsImplementation();
#pragma warning restore IDE0022 // Use expression body for methods
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");

    }
}
