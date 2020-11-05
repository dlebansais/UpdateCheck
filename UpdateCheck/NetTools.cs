namespace UpdateCheck
{
    using System;
    using System.Net;

    /// <summary>
    /// Tools to manage Internet connections.
    /// </summary>
    internal static class NetTools
    {
        /// <summary>
        /// Changes the security protocol to use TLS 1.2.
        /// </summary>
        /// <param name="oldSecurityProtocol">The previous settings upon return.</param>
        public static void EnableSecurityProtocol(out object oldSecurityProtocol)
        {
            oldSecurityProtocol = new Tuple<bool, SecurityProtocolType>(ServicePointManager.Expect100Continue, ServicePointManager.SecurityProtocol);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Changes the security protocol to use TLS 1.2.
        /// </summary>
        /// <param name="oldSecurityProtocol">The settings from <see cref="EnableSecurityProtocol(out object)"/>.</param>
        public static void RestoreSecurityProtocol(object oldSecurityProtocol)
        {
            if (oldSecurityProtocol is Tuple<bool, SecurityProtocolType> RestoredValues)
            {
                ServicePointManager.Expect100Continue = RestoredValues.Item1;
                ServicePointManager.SecurityProtocol = RestoredValues.Item2;
            }
        }
    }
}
