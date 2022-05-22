namespace UEFI
{
    public static class UefiNamespaces
    {
        public const string EFI_GLOBAL_VARIABLE = "{8BE4DF61-93CA-11D2-AA0D-00E098032B8C}";
        public const string EFI_SIMPLE_TEXT_INPUT_PROTOCOL_GUID = "{387477C1-69C7-11D2-8E39-00A0C969723B}";
        public const string EFI_LOAD_FILE_PROTOCOL_GUID = "{56EC3091-954C-11D2-8E3F-00A0C969723B}";
        public const string EFI_SIMPLE_NETWORK_PROTOCOL_GUID = "{A19832B9-AC25-11D3-9A2D-0090273FC14D}";
        public const string EFI_MANAGED_NETWORK_SERVICE_BINDING_PROTOCOL_GUID = "{F36FF770-A7E1-42CF-9ED2-56F0F271F44C}";
        public const string EFI_ARP_SERVICE_BINDING_PROTOCOL_GUID = "{F44C00EE-1F2C-4A00-AA09-1C9F3E0800A3}";
        public const string EFI_ARP_PROTOCOL_GUID = "{F4B427BB-BA21-4F16-BC4E-43E416AB619C}";
        public const string EFI_SERIAL_IO_PROTOCOL_GUID = "{BB25CF6F-F1D4-11D2-9A0C-0090273FC1FD}";
        public const string EFI_DEVICE_PATH_PROTOCOL_GUID = "{09576E91-6D3F-11D2-8E39-00A0C969723B}";
        public const string EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL_GUID = "{387477C2-69C7-11D2-8E39-00A0C969723B}";
        public const string EFI_SIMPLE_FILE_SYSTEM_PROTOCOL_GUID = "{964E5B22-6459-11D2-8E39-00A0C969723B}";
        public const string EFI_DISK_IO_PROTOCOL_GUID = "{CE345171-BA0B-11D2-8E4F-00A0C969723B}";
        public const string EFI_BLOCK_IO_PROTOCOL_GUID = "{964E5B21-6459-11D2-8E39-00A0C969723B}";
        public const string EFI_UNICODE_COLLATION_PROTOCOL_GUID = "{1D85CD7F-F43D-11D2-9A0C-0090273FC14D}";
        public const string EFI_NETWORK_INTERFACE_IDENTIFIER_PROTOCOL_GUID = "{E18541CD-F755-4F73-928D-643C8A79B229}";
        public const string EFI_PXE_BASE_CODE_PROTOCOL_GUID = "{03C4E603-AC28-11D3-9A2D-0090273FC14D}";
        public const string EFI_PXE_BASE_CODE_CALLBACK_PROTOCOL_GUID = "{245DCA21-FB7B-11D3-8F01-00A0C969723B}";
        public const string EFI_MANAGED_NETWORK_PROTOCOL_GUID = "{3B95AA31-3793-434B-8667-C8070892E05E}";
        public const string EFI_DHCP4_PROTOCOL_GUID = "{8A219718-4EF5-4761-91C8-C0F04BDA9E56}";
        public const string EFI_DHCP4_SERVICE_BINDING_PROTOCOL_GUID = "{9D9A39D8-BD42-4A73-A4D5-8EE94BE11380}";
        public const string EFI_TCP4_PROTOCOL_GUID = "{65530BC7-A359-410F-B010-5AADC7EC2B62}";
        public const string EFI_TCP4_SERVICE_BINDING_PROTOCOL_GUID = "{00720665-67EB-4A99-BAF7-D3C33A1C7CC9}";
        public const string EFI_IP4_PROTOCOL_GUID = "{41D94CD2-35B6-455A-8258-D4E51334AADD}";
        public const string EFI_IP4_SERVICE_BINDING_PROTOCOL_GUID = "{C51711E7-B4BF-404A-BFB8-0A048EF1FFE4}";
        public const string EFI_IP4_CONFIG_PROTOCOL_GUID = "{3B95AA31-3793-434B-8667-C8070892E05E}";
        public const string EFI_UDP4_PROTOCOL_GUID = "{3AD9DF29-4501-478D-B1F8-7F7FE70E50F3}";
        public const string EFI_UDP4_SERVICE_BINDING_PROTOCOL_GUID = "{83F01464-99BD-45E5-B383-AF6305D8E9E6}";
        public const string EFI_MTFTP4_PROTOCOL_GUID = "{78247C57-63DB-4708-99C2-A8B4A9A61F6B}";
        public const string EFI_MTFTP4_SERVICE_BINDING_PROTOCOL_GUID = "{2FE800BE-8F01-4AA6-946B-D71388E1833F}";
        public const string EFI_AUTHENTICATION_CHAP_RADIUS_GUID = "{D6062B50-15CA-11DA-9219-001083FFCA4D}";
        public const string EFI_AUTHENTICATION_CHAP_LOCAL_GUID = "{C280C73E-15CA-11DA-B0CA-001083FFCA4D}";

        public const string WINDOWS = "{77FA9ABD-0359-4D32-BD60-28F4E78F784B}";
        public const string SURFACE = "{D2E0B9C9-9860-42CF-B360-F906D5E0077A}";
        public const string TESTING = "{1801FBE3-AEF7-42A8-B1CD-FC4AFAE14716}";
        public const string SECURITY_DATABASE = "{d719b2cb-3d3a-4596-a3bc-dad00e67656f}";

        #region Third Party

        public const string SYSTEMD_BOOT = "{4a67b082-0a4c-41cf-b6c7-440b29bb8c4f}";
        public const string APPLE_VENDOR = "{4D1EDE05-38C7-4A6A-9CC6-4BCCA8B38C14}";
        public const string APPLE_BOOT = "{7C436110-AB2A-4BBB-A880-FE41995C9F82}";
        public const string Apple_Hardware_Configuration_Storage_for_MacPro7_1 = "{5EDDA193-A070-416A-85EB-2A1181F45B18}";
        public const string OC_VENDOR = "{4D1FDA02-38C7-4A6A-9CC6-4BCCA8B30102}";

        #endregion

        public static Dictionary<string, string> HumanReadableNames = new(StringComparer.OrdinalIgnoreCase)
        {
            { EFI_GLOBAL_VARIABLE, "EFI Global Variable"},
            { EFI_SIMPLE_TEXT_INPUT_PROTOCOL_GUID, "EFI Simple Text Input Protocol" },
            { EFI_LOAD_FILE_PROTOCOL_GUID, "EFI Load File Protocol"},
            { EFI_SIMPLE_NETWORK_PROTOCOL_GUID, "EFI Simple Network Protocol"},
            { EFI_MANAGED_NETWORK_SERVICE_BINDING_PROTOCOL_GUID, "EFI Managed Network Service Binding Protocol"},
            { EFI_ARP_SERVICE_BINDING_PROTOCOL_GUID, "EFI ARP Service Binding Protocol"},
            { EFI_ARP_PROTOCOL_GUID, "EFI ARP Protocol"},
            { EFI_SERIAL_IO_PROTOCOL_GUID, "EFI Serial IO Protocol"},
            { EFI_DEVICE_PATH_PROTOCOL_GUID, "EFI Device Path Protocol"},
            { EFI_SIMPLE_TEXT_OUTPUT_PROTOCOL_GUID, "EFI Simple Text Output Protocol"},
            { EFI_SIMPLE_FILE_SYSTEM_PROTOCOL_GUID, "EFI Simple File System Protocol"},
            { EFI_DISK_IO_PROTOCOL_GUID, "EFI Disk I/O Protocol"},
            { EFI_BLOCK_IO_PROTOCOL_GUID, "EFI Block I/O Protocol"},
            { EFI_UNICODE_COLLATION_PROTOCOL_GUID, "EFI Unicode Collation Protocol"},
            { EFI_NETWORK_INTERFACE_IDENTIFIER_PROTOCOL_GUID, "EFI Network Interface Identifier Protocol"},
            { EFI_PXE_BASE_CODE_PROTOCOL_GUID, "EFI PXE Base Code Protocol"},
            { EFI_PXE_BASE_CODE_CALLBACK_PROTOCOL_GUID, "EFI PXE Base Code Callback Protocol"},
            { EFI_MANAGED_NETWORK_PROTOCOL_GUID, "EFI Managed Network Protocol and EFI IP4 Configuration Protocol"},
            { EFI_DHCP4_PROTOCOL_GUID, "EFI DHCP4 Protocol"},
            { EFI_DHCP4_SERVICE_BINDING_PROTOCOL_GUID, "EFI DHCP4 Service Binding Protocol"},
            { EFI_TCP4_PROTOCOL_GUID, "EFI TCP4 Protocol"},
            { EFI_TCP4_SERVICE_BINDING_PROTOCOL_GUID, "EFI TCP4 Service Binding Protocol"},
            { EFI_IP4_PROTOCOL_GUID, "EFI IP4 Protocol"},
            { EFI_IP4_SERVICE_BINDING_PROTOCOL_GUID, "EFI IP4 Service Binding Protocol"},
            // { EFI_IP4_CONFIG_PROTOCOL_GUID, "EFI IP4 Configuration Protocol"},
            { EFI_UDP4_PROTOCOL_GUID, "EFI UDP4 Protocol"},
            { EFI_UDP4_SERVICE_BINDING_PROTOCOL_GUID, "EFI UDP4 Service Binding Protocol"},
            { EFI_MTFTP4_PROTOCOL_GUID, "EFI MTFTP4 Protocol"},
            { EFI_MTFTP4_SERVICE_BINDING_PROTOCOL_GUID, "EFI MTFTP4 Service Binding Protocol"},
            { EFI_AUTHENTICATION_CHAP_RADIUS_GUID, "EFI Authentication CHAP Radius"},
            { EFI_AUTHENTICATION_CHAP_LOCAL_GUID, "EFI Authentication CHAP Local"},
            { WINDOWS, "Windows"},
            { SURFACE, "Surface"},
            { TESTING, "Testing"},
            { SECURITY_DATABASE, "Security Database"},
            { SYSTEMD_BOOT, "Systemd Boot"},
            { APPLE_VENDOR, "Apple Vendor"},
            { APPLE_BOOT, "Apple Boot"},
            { Apple_Hardware_Configuration_Storage_for_MacPro7_1, "Apple Hardware Configuration Storage for MacPro7.1"},
            { OC_VENDOR, "OC Vendor"},
        };
    }
}