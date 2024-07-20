﻿namespace dotLNKLibrary;

public sealed class CLSID
{
    public static Dictionary<string, string> Items => new()
    {
        { "Add Network Location", "{d4480a50-ba28-11d1-8e75-00c04fa31a86}" },
        { "Administrative Tools", "{d20ea4e1-3957-11d2-a40b-0c5020524153}" },
        { "Applications", "{4234d49b-0245-4df3-b780-3893943456e1}" },
        { "Autoplay", "{9c60de1e-e5fc-40f4-a487-460851a8d915}" },
        { "Backup And Restore", "{b98a2bea-7d42-4558-8bd1-832f41bac6fd}" },
        { "Bitlocker Drive Encryption", "{d9ef8727-cac2-4e60-809e-86f80a666c91}" },
        { "Bluetooth Devices", "{28803f59-3a75-4058-995f-4ee5503b023c}" },
        { "Color Management", "{b2c761c6-29bc-4f19-9251-e6195265baf1}" },
        { "Command Folder", "{437ff9c0-a07f-4fa0-af80-84b6c6440a16}" },
        { "Common Places", "{d34a6ca6-62c2-4c34-8a7c-14709c1ad938}" },
        { "Control Panel", "{5399e694-6ce5-4d6c-8fce-1d8870fdcba0}" },
        { "Credential Manager", "{1206f5f1-0569-412c-8fec-3204630dfb70}" },
        { "Date And Time", "{e2e7934b-dce5-43c4-9576-7fe4f75e7480}" },
        { "Default Programs", "{17cd9488-1228-4b2f-88ce-4298e93e0966}" },
        { "Desktop Folder", "{b4bfcc3a-db2c-424c-b029-7fe99a87c641}" },
        { "Device Manager", "{74246bfc-4c96-11d0-abef-0020af6b0b7a}" },
        { "Devices And Printers", "{a8a91a66-3a7d-4424-8d24-04e180695c7a}" },
        { "Display", "{c555438b-3c23-4769-a71f-b6d3d9b6053a}" },
        { "Documents Folder", "{a8cdff1c-4878-43be-b5fd-f8091c1c60d0}" },
        { "Downloads Folder", "{374de290-123f-4565-9164-39c4925e467b}" },
        { "Ease Of Access Center", "{d555645e-d4f8-4c29-a827-d93c859c4f2a}" },
        { "E-Mail Program", "{2559a1f5-21d7-11d4-bdaf-00c04f60b9f0}" },
        { "Family Safety", "{96ae8d84-a250-4520-95a5-a47a7e3c548b}" },
        { "Favorites", "{323ca680-c24d-4099-b94d-446dd2d7249e}" },
        { "File Explorer Options", "{6dfd7c5c-2451-11d3-a299-00c04f8ef6af}" },
        { "File History", "{f6b6e965-e9b2-444b-9286-10c9152edbc5}" },
        { "Font Settings", "{93412589-74d4-4e4e-ad0e-e0cb621440fd}" },
        { "Fonts Folder", "{bd84b380-8ca2-1069-ab1d-08000948f534}" },
        { "Frequent Folders", "{3936e9e4-d92c-4eee-a85a-bc16d5ea0819}" },
        { "Games Explorer", "{ed228fdf-9ea8-4870-83b1-96b02cfe0d52}" },
        { "Get Programs", "{15eae92e-f17a-4431-9f28-805e482dafd4}" },
        { "Help And Support", "{2559a1f1-21d7-11d4-bdaf-00c04f60b9f0}" },
        { "Homegroup Settings", "{67ca7650-96e6-4fdd-bb43-a8e774f73a57}" },
        { "Homegroup Users", "{b4fb3f98-c1ea-428d-a78a-d1f5659cba93}" },
        { "Hyper-V Remote File Browsing", "{0907616e-f5e6-48d8-9d61-a91c3d28106d}" },
        { "Indexing Options", "{87d66a43-7b11-4a28-9811-c86ee395acf7}" },
        { "Installed Updates", "{d450a8a1-9568-45c7-9c0e-b4f9fb4537bd}" },
        { "Internet Options In Internet Explorer", "{a3dd4f92-658a-410f-84fd-6fbbbef2fffe}" },
        { "Keyboard Properties", "{725be8f7-668e-4c7b-8f90-46bdb0936430}" },
        { "Language Settings", "{bf782cc9-5a52-4a17-806c-2a894ffeeac5}" },
        { "Libraries", "{031e4825-7b94-4dc3-b131-e946b44c8dd5}" },
        { "Location Information", "{40419485-c444-4567-851a-2dd7bfa1684d}" },
        { "Location Settings", "{e9950154-c418-419e-a90a-20c5287ae24b}" },
        { "Media Servers", "{289af617-1cc3-42a6-926c-e6a863f0e3ba}" },
        { "Mouse Properties", "{6c8eec18-8d75-41b2-a177-8831d59d2d50}" },
        { "Music Folder", "{1cf1260c-4dd0-4ebb-811f-33c572699fde}" },
        { "My Documents", "{450d8fba-ad25-11d0-98a8-0800361b1103}" },
        { "Network", "{f02c1a0d-be21-4350-88b0-7367fc96ef3c}" },
        { "Network And Sharing Center", "{8e908fc9-becc-40f6-915b-f4ca0e70d03d}" },
        { "Network Connections In Pc Settings", "{38a98528-6cbf-4ca9-8dc0-b1e1d10f7b1b}" },
        { "Network Connections", "{7007acc7-3202-11d1-aad2-00805fc1270e}" },
        { "Network Workgroup", "{208d2c60-3aea-1069-a2d7-08002b30309d}" },
        { "Notification Area Icons", "{05d7b0f4-2121-4eff-bf6b-ed3f69b894d9}" },
        { "Nvidia Control Panel", "{0bbca823-e77d-419e-9a44-5adec2c8eeb0}" },
        { "Offline Files Folder", "{afdb1f70-2a4c-11d2-9039-00c04f8eeb3e}" },
        { "Onedrive", "{018d5c66-4533-4307-9b53-224de2ed1fe6}" },
        { "Pen And Touch", "{f82df8f7-8b9f-442e-a48c-818ea735ff9b}" },
        { "Personalization", "{ed834ed6-4b5a-4bfe-8f11-a626dcb6a921}" },
        { "Pictures Folder", "{3add1653-eb32-4cb0-bbd7-dfa0abb5acca}" },
        { "Portable Devices", "{35786d3c-b075-49b9-88dd-029876e11c01}" },
        { "Power Options", "{025a5937-a6be-4686-a844-36fe4bec8b6d}" },
        { "Previous Versions Results Folder", "{f8c2ab3b-17bc-41da-9758-339d7dbf2d88}" },
        { "Printhood Delegate Folder", "{ed50fc29-b964-48a9-afb3-15ebb9b97f36}" },
        { "Printers", "{2227a280-3aea-1069-a2de-08002b30309d}" },
        { "Programs And Features", "{7b81be6a-ce2b-4676-a29e-eb907a5126c5}" },
        { "Public Folder", "{4336a54d-038b-4685-ab02-99bb52d3fb8b}" },
        { "Quick Access", "{679f85cb-0220-4080-b29b-5540cc05aab6}" },
        { "Recent Places", "{22877a6d-37a1-461a-91b0-dbda5aaebc99}" },
        { "Recovery", "{9fe63afd-59cf-4419-9775-abcc3849f861}" },
        { "Recycle Bin", "{645ff040-5081-101b-9f08-00aa002f954e}" },
        { "Region And Language", "{62d8ed13-c9d0-4ce8-a914-47dd628fb1b0}" },
        { "Remoteapp And Desktop Connections", "{241d7c96-f8bf-4f85-b01f-e2b043341a4b}" },
        { "Remote Printers", "{863aa9fd-42df-457b-8e4d-0de1b8015c60}" },
        { "Removable Storage Devices", "{a6482830-08eb-41e2-84c1-73920c2badb9}" },
        { "Results Folder", "{2965e715-eb66-4719-b53f-1672673bbefa}" },
        { "Run", "{2559a1f3-21d7-11d4-bdaf-00c04f60b9f0}" },
        { "Search", "{9343812e-1c37-4a49-a12e-4b2d810d956b}" },
        { "Search Everywhere", "{2559a1f8-21d7-11d4-bdaf-00c04f60b9f0}" },
        { "Search Files", "{2559a1f0-21d7-11d4-bdaf-00c04f60b9f0}" },
        { "Security And Maintenance", "{bb64f8a7-bee7-4e1a-ab8d-7d8273f7fdb6}" },
        { "Set Program Access And Computer Defaults", "{2559a1f7-21d7-11d4-bdaf-00c04f60b9f0}" },
        { "Show Desktop", "{3080f90d-d7ad-11d9-bd98-0000947b0257}" },
        { "Sound", "{f2ddfc82-8f12-4cdd-b7dc-d4fe1425aa4d}" },
        { "Speech Recognition", "{58e3c745-d971-4081-9034-86e34b30836a}" },
        { "Storage Spaces", "{f942c606-0914-47ab-be56-1321b8035096}" },
        { "Sync Center", "{9c73f5e5-7ae7-4e32-a8e8-8d23b85255bf}" },
        { "Sync Setup Folder", "{2e9e59c0-b437-4981-a647-9c34b9b90891}" },
        { "System", "{bb06c0e4-d293-4f75-8a90-cb05b6477eee}" },
        { "System Icons", "{05d7b0f4-2121-4eff-bf6b-ed3f69b894d9}" },
        { "Tablet Pc Settings", "{80f3f1d5-feca-45f3-bc32-752c152e456e}" },
        { "Taskbar And Navigation Properties", "{0df44eaa-ff21-4412-828e-260a8728e7f1}" },
        { "Text To Speech", "{d17d1d6d-cc3f-4815-8fe3-607e7d5d10b3}" },
        { "This PC", "{20d04fe0-3aea-1069-a2d8-08002b30309d}" },
        { "Troubleshooting", "{c58c4893-3be0-4b45-abb5-a63e4b8c8651}" },
        { "User Accounts", "{60632754-c523-4b62-b45c-4172da012619}" },
        { "User Accounts (Netplwiz)", "{7a9d77bd-5403-11d2-8785-2e0420524153}" },
        { "User Pinned", "{1f3427c8-5c10-4210-aa03-2ee45287d668}" },
        { "%Userprofile%", "{59031a47-3f72-44a7-89c5-5595fe6b30ee}" },
        { "Videos Folder", "{a0953c92-50dc-43bf-be83-3742fed03c9c}" },
        { "Web Browser", "{871c5380-42a0-1069-a2ea-08002b30309d}" },
        { "Windows Defender", "{d8559eb9-20c0-410e-beda-7ed416aecc2a}" },
        { "Windows Mobility Center", "{5ea4f148-308c-46d7-98a9-49041b1dd468}" },
        { "Windows Features", "{67718415-c450-4f3c-bf8a-b487642dc39b}" },
        { "Windows Firewall", "{4026492f-2f69-46b8-b9bf-5654fc07e423}" },
        { "Windows To Go", "{8e0c279d-0bd1-43c3-9ebd-31c3dc5b8a77}" },
        { "Windows Update", "{36eef7db-88ad-4e81-ad49-0e313f0c35f8}" },
        { "Work Folders", "{ecdb0924-4208-451e-8ee0-373c0956de16}" }
    };
}