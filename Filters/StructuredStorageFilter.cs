﻿using System;
using System.Collections.Generic;
using System.IO;
using HistoryFilter.Util;
using log4net;
using OpenMcdf;

namespace HistoryFilter.Filters {
    class StructuredStorageFilter : IFilter {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StructuredStorageFilter));
        private List<string> _masks;
        private readonly Shell32.Shell _shell = new Shell32.Shell();

        #region ApplicationMapping
        private static readonly Dictionary<string, string> ApplicationMapping = new Dictionary<string, string> { {"0006f647f9488d7a", "AIM 7.5.11.9 (custom AppID + JL support)"},
            {"00098b0ef1c84088", "fulDC 6.78"},
            {"012dc1ea8e34b5a6", "Microsoft Paint 6.1"},
            {"01b29f0dc90366bb", "AIM 5.9.3857"},
            {"03d877ec11607fe4", "Thunderbird 6.0.2"},
            {"044a50e6c87bc012", "Classic FTP Plus 2.15"},
            {"050620fe75ee0093", "VMware Player 3.1.4"},
            {"05e01ecaf82f7d8e", "Scour Exchange 0.0.0.228"},
            {"06059df4b02360af", "Kadu 0.10.0 / 0.6.5.5"},
            {"070b52cf73249257", "Sococo 1.5.0.2274"},
            {"0a1d19afe5a80f80", "FileZilla 2.2.32"},
            {"0a79a7ce3c45d781", "CuteFTP 7.1 (Build 06.06.2005.1)"},
            {"0b17d3d0c9ca7e29", "Document ViewerPicasa 3.8.0 (Build 117.43, 0)"},
            {"0b3f13480c2785ae", "Paint 6.1 (build 7601: SP1)"},
            {"0b48ce76eda60b97", "Shareaza 8.0.0.112300"},
            {"0cfab0ec14b6f953", "Express NewsPictures 2.41 (Build 08.05.07.0)"},
            {"0ef606b196796ebb", "HP MediaSmart Photo"},
            {"10f5a20c21466e85", "FTP Voyager 15.2.0.17"},
            {"1110d9896dceddb3", "imgSeek 0.8.5"},
            {"12dc1ea8e34b5a6", "Microsoft Paint 6.1"},
            {"134620458666ccb0", "TeraCopy 2.3 64-bit"},
            {"135df2a440abe9bb", "SoulSeek 156c"},
            {"13eb0e5d9a49eaef", "Binjet 3.0.2"},
            {"1434d6d62d64857d", "BitLord 1.2.0-66"},
            {"14354e216395983a", "Remote Desktop Manager 2.1.0.0 FREE"},
            {"1461132e553e2e6c", "Firefox 6.0"},
            {"169b3be0bc43d592", "FastPictureViewer Professional 1.6 (Build 211)"},
            {"16d71406474462b5", "Snagit Editor 12.4.1"},
            {"16ec093b8f51508f", "Opera 8.54 build 7730 / 9.64 build 10487 / 11.50 build 1074"},
            {"16f2f0042ddbe0e8", "Windows Terminal"},
            {"174c2c811c286c58", "InfraRecorder 0.53.0.0 64-bit"},
            {"177aeb41deb606ae", "Adobe Photoshop CS6 (64 Bit)"},
            {"17d3eb086439f0d7", "TrueCrypt 7.0a"},
            {"18434d518c3a61eb", "Minitab 17"},
            {"186b5ccada1d986b", "NewsGrabber 3.0.36"},
            {"188f5ec9d11ded56", "Microsoft Edge (Chromium)"},
            {"19ccee0274976da8", "mIRC 4.72 / 5.61"},
            {"19e6043495a5b4da", "Edit Pad Pro"},
            {"1a60b1067913516a", "Psi 0.14"},
            {"1a89d1befe8e90e3", "Adobe Acrobat Distiller Pro XI 32-bit 11.0.0779"},
            {"1b29f0dc90366bb", "AIM 5.9.3857"},
            {"1b4dd67f29cb1962", "Windows Explorer (Win7)"},
            {"1bc392b8e104a00e", "Remote Desktop"},
            {"1bc9bbbe61f14501", "OneNote"},
            {"1c30573bdfce4155", "Zenmap GUI 6.49BETA4"},
            {"1c7a9be1b15a03ba", "Microsoft ScreenSketch"},
            {"1ced32d74a95c7bc", "Microsoft Visual Studio Code"},
            {"1cf97c38a5881255", "MediaPortal 1.1.3"},
            {"1cffbe973a437c74", "DSPlayer 0.889 Lite"},
            {"1d12f965b876dc87", "Snagit 2021"},
            {"1da3c90a72bf5527", "Safari 4.0.5 (531.22.7) / 5.1 (7534.50)"},
            {"1eb796d87c32eff9", "Firefox 5.0"},
            {"20513cdf29d09c0e", "Hex Editor Neo"},
            {"20ef367747c22564", "Bullet Proof FTP 2010.75.0.75"},
            {"223bf0f360c6fea5", "I2P 0.8.8 (restartable)"},
            {"226400522157fe8b", "FileZilla Server 0.9.39 beta"},
            {"22c4d315e96389e0", "FastCopy 3.12"},
            {"22cefa022402327d", "Meca Messenger 5.3.0.52"},
            {"23646679aaccfae0", "Adobe Reader 9.x"},
            {"23709f6439b9f03d", "Hex Editor Neo 5.14"},
            {"23ef200ca6364eff", "Oracle VM VirtualBox 5.0.16"},
            {"23f08dab0f6aaf30", "SoMud 1.3.3"},
            {"2417caa1f2a881d4", "ICQ 7.6 (Build 5617)"},
            {"2437d4d14b056114", "EiskaltDC++ 2.2.3"},
            {"2519133d6d830f7e", "IMatch 3.6.0.113"},
            {"2544ff74641b639d", "WiseFTP 6.1.5"},
            {"26717493b25aa6e1", "Adobe Dreamweaver CS5 (32-bit)"},
            {"26753c97ea000ecd", "LibreOffice 5.1.0.3 Math"},
            {"271e609288e1210a", "Microsoft Office Access 2010 x86"},
            {"27da120d7e75cf1f", "pbFTPClient 6.1"},
            {"27ececd8d89b6767", "AIM 6.2.14.2 / 6.5.3.12 / 6.9.17.2"},
            {"28493d9d08e13aa6", "UltraVNC Viewer 1.2.1.0"},
            {"28c8b86deab549a1", "Internet Explorer 8 / 9 / 10"},
            {"28efb5b6d2e28389", "EA Origin"},
            {"290532160612e071", "WinRar"},
            {"292a746334889a7e", "SQLiteSpy 1.9.13"},
            {"2a5a615382a84729", "X-Chat 2 2.8.6-2"},
            {"2aa756186e21b320", "RealTimeQuery 3.2"},
            {"2b164f512891ae37", "NewsWolf NSListGen"},
            {"2b53c4ddf69195fc", "Zune x64"},
            {"2b5841989b3857da", "RealVNC Server 5.3.0 64-bit (Chat)"},
            {"2ca2a1a69dc5465f", "UltraVNC 1.2.1.0 Server Property Page"},
            {"2d1658d5dc3cbe2d", "MySpaceIM 1.0.823.0 Beta"},
            {"2d61cccb4338dfc8", "BitTorrent 5.0.0 / 6.0.0 / 7.2.1 (Build 25548)"},
            {"2db8e25112ab4453", "Deluge 1.3.12 / 1.3.3"},
            {"2fa14c7753239e4c", "Paint.NET 2.72 / 3.5.8.4081.24580"},
            {"2ff9dc8fb7e11f39", "I2P 0.8.8 (no window)"},
            {"3094cdb43bf5e9c2", "Microsoft Office OneNote 2010 x86"},
            {"30d23723bdd5d908", "Digsby (Build 30140) (JL support)"},
            {"315e29a36e961336", "Roboform 7.8"},
            {"3168cc975b354a01", "Slypheed 3.1.2 (Build 1120)"},
            {"3198e37206f28dc7", "CuteFTP 8.3 Professional (Build 8.3.4.0007)"},
            {"319f01bf9fe00f2d", "Microsoft Access 64-bit"},
            {"31e8ac6b0784ed7d", "Foxit Reader 9.4.0.16811"},
            {"3353b940c074fd0c", "Microsoft Built-in Snipping Tool"},
            {"337ed59af273c758", "Sticky Notes"},
            {"3461e4d1eb393c9c", "WTW 0.8.18.2852 / 0.8.19.2940"},
            {"352fd027c0e8f0e5", "Zoom"},
            {"353e9052cccbec5d", "Kindle for PC 1.21.0"},
            {"3594aab44bca414b", "Windows Photo Viewer"},
            {"36801066f71b73c5", "Binbot 2.0"},
            {"36c36598b08891bf", "Vovox 2.5.3.4250"},
            {"36f6bc3efe1d99e0", "Alt.Binz 0.25.0 (Build 27.09.2007)"},
            {"37392221756de927", "RealPlayer SP 12"},
            {"3866ff352d7719e1", "Paint.NET 4.0.9"},
            {"386a2f6aa7967f36", "EyeBrowse 2.7"},
            {"387d72eb9c9aa960", "UltraVNC 1.2.1.0 Launcher"},
            {"3917dd550d7df9a8", "Konvertor 4.06 (Build 10)"},
            {"3a5148bf2288a434", "Secure FTP 2.6.1 (Build 20101209.1254)"},
            {"3b94415067dd2c5d", "GOG Galaxy"},
            {"3be7b307dfccb58f", "NiouzeFire 0.8.7.0"},
            {"3c0022d9de573095", "QuteCom 2.2"},
            {"3c309c17f7e8ffe1", "GIMP 2.8.16"},
            {"3c3871276e149215", "PowerShell 7"},
            {"3c93a049a30e25e6", "J. River Media Center 16.0.149"},
            {"3cf13d83b0bd3867", "RevConnect 0.674p (based on DC++)"},
            {"3d877ec11607fe4", "Thunderbird 6.0.2"},
            {"3dc02b55e44d6697", "7-Zip 3.13 / 4.20"},
            {"3df22b7648cec4c1", "TeamViewer 11.0.55321"},
            {"3e9850346f375d41", "Foxit Phantom PDF 7.2.2.929"},
            {"3ed70ef3495535f7", "Gravity 3.0.4"},
            {"3edf100b207e2199", "digiKam 1.7.0 (KDE 4.4.4)"},
            {"3f2cd46691bbee90", "GOIM 1.1.0"},
            {"3f97341a65bac63a", "Ozum 6.07 (Build 6070)"},
            {"409b67100697bcc0", "Revo Uninstaller Pro 3.1.5"},
            {"40f2aca05d8a33f2", "Minitab 16"},
            {"411447f7de177c68", "Windows DVD Maker 64-bit (Win7)"},
            {"4278d3dc044fc88a", "Gaim 1.5.0"},
            {"431a5b43435cc60b", "Python (.pyc)"},
            {"43578521d78096c6", "Windows Media Player Classic Home Cinema 1.3 (32-bit)"},
            {"435a2f986b404eb7", "SmartFTP"},
            {"43886ba3395acdcc", "Easy Post 3.0"},
            {"447e6aa2bbdfbc8a", "Slack 4.11.3"},
            {"44a3621b32122d64", "Microsoft Office Word 2010 x64"},
            {"44a398496acc926d", "Adobe Premiere Pro CS5 (64-bit)"},
            {"44a50e6c87bc012", "Classic FTP Plus 2.15"},
            {"454ef7dca3bb16b2", "Exodus 0.10.0.0"},
            {"466d339d8f21cfbf", "Microsoft Snip & Sketch"},
            {"469e4a7982cea4d4", "Windows Wordpad"},
            {"46f433176bc0b3d2", "WinRAR 5.30 beta 64-bit"},
            {"4700ff5ae80a6713", "PDFCreator 2.2"},
            {"490c000889535727", "WinMX 4.9.3.0"},
            {"4975d6798a8bdf66", "7-Zip 4.65 / 9.20"},
            {"497b42680f564128", "Zoner PhotoStudio 13 (Build 7)"},
            {"49b5edbd92d8cd58", "FTP Commander 8.02"},
            {"49db7ed4f2703c22", "LogMeIn Client 1.3.1835"},
            {"4a49906d074a3ad3", "Media Go 1.8 (Build 121)"},
            {"4a7e4f6a181d3d08", "broolzShare"},
            {"4aa2a5710da3efe0", "DCSharpHub 2.0.0"},
            {"4ac866364817f10c", "Microsoft Edge (Chromium)"},
            {"4acae695c73a28c7", "VLC 0.3.0 / 0.4.6"},
            {"4b632cf2ceceac35", "Robo-FTP Server 3.2.5"},
            {"4b6925efc53a3c08", "BCWipe Task Manager"},
            {"4b8a4727aa452343", "Firefox 56.0.2"},
            {"4c58cf9096ef3efd", "Kindle for PC 1.24.3 "},
            {"4cb9c5750d51c07f", "Microsoft Movies & TV (Build 10.19031.11411.0)"},
            {"4cdf7858c6673f4b", "Bullet Proof FTP 1.26"},
            {"4d72cfa1d0a67418", "Newsgroup Image Collector"},
            {"4d7bdaea55ad352", "PeaZip 6.0.0"},
            {"4d8bdacf5265a04f", "The KMPlayer 2.9.4.1434"},
            {"4dd48f858b1a6ba7", "Free Download Manager 3.0 (Build 852)"},
            {"4e0ac37db19cba15", "Xfire 1.138 (Build 44507)"},
            {"4e538fde985a3c01", "Torch Browser 65.0.0.1614 (x86)"},
            {"4f24a7b84a7de5a6", "Palringo 2.6.3 (r45983)"},
            {"4fceec8e021ac978", "CoffeeCup Free FTP 3.5.0.0"},
            {"4fd44f9938892caa", "CDBurnerXP"},
            {"500b8c1d5302fc9c", "Python (.pyw)"},
            {"50620fe75ee0093", "VMware Player"},
            {"50c5e019818564e3", "Microsoft Excel Viewer 12.0.6219.1000"},
            {"521a29e5d22c13b4", "Skype 1.4.0.84 / 2.5.0.154 / 3.8.0.139 / 4.2.0.187 / Skype 5.3.0.120 / 5.5.0.115 / 5.5.32.117"},
            {"54c803dfc87b52ba", "Nettalk 6.7.12"},
            {"550abc1cb58eb92c", "VeraCrypt"},
            {"558c5bd9f906860a", "BearShare Lite 5.2.5.1"},
            {"560d789a6a42ad5a", "DC++ 0.261 / 0.698 / 0.782 (r2402.1)"},
            {"56c5204009d2b915", "uTorrent 3.5.5"},
            {"573770283dc3d854", "Windows Defender"},
            {"58170c92fa4b91a1", "MediaMonkey"},
            {"590aee7bdd69b59b", "Windows Powershell "},
            {"59e86071b87ac1c3", "CuteFTP 8.3 (Build 8.3.4.0007)"},
            {"59f56184c796cfd4", "ACDSee Photo Manager 10 (Build 219)"},
            {"5b186fc4a0b40504", "Dtella 1.2.5 (Purdue network only)"},
            {"5b72f67adcce9045", "UltraVNC 1.2.1.0 Settings"},
            {"5b7f3287093c1623", "Total Commander 8.52a 64-bit"},
            {"5bb830f67194431a", "7-Zip 18.05 (x64)"},
            {"5c450709f7ae4396", "Firefox"},
            {"5d696d521de238c3", "Chrome"},
            {"5d6f13ed567aa2da", "Microsoft Office Outlook 2010 x64"},
            {"5d7b4175afdcc260", "Shareaza 2.0.0.0"},
            {"5da8f997fd5f9428", "Internet Explorer x64"},
            {"5df4765359170e26", "Firefox 4.0.1"},
            {"5e01ecaf82f7d8e", "Scour Exchange 0.0.0.228"},
            {"5ea2a50c7979fbdc", "TrustyFiles 3.1.0.22"},
            {"5f218922e0901ebf", "MusicBee"},
            {"5f6e7bc0fb699772", "Microsoft Office PowerPoint 2010 x64"},
            {"5f7b5f1e01b83767", "Quick Access"},
            {"5fb817cd5a8cad21", "Google Drive"},
            {"5fd959f6fe6b8ae7", "PuTTY 0.70 (x64)"},
            {"6059df4b02360af", "Kadu 0.10.0 / 0.6.5.5"},
            {"606a33f5a27b57d4", "Microsoft Built-in Computer Management 10.0.10011.16384 (Win10)"},
            {"6224453d9701a612", "BinTube 3.7.1.0 (requires VLC 10.5!)"},
            {"62bff50b969c2575", "Quintessential Media Player "},
            {"62dba7fb39bb0adc", "Yahoo Messenger 7.5.0.647 / 8.1.0.421 / 9.0.0.2162 / 10.0.0.1270"},
            {"65009083bfa6a094", "(app launched via XPMode)"},
            {"65f7dd884b016ab2", "LimeChat 2.39"},
            {"669967f27afdebec", "NirSoft PstPassword 1.20 (x86)"},
            {"6728dd69a3088f97", "Windows Command Processor - cmd.exe"},
            {"6824f4a902c78fbd", "Firefox 64.0"},
            {"689319b6547cda85", "emesene 2.11.7"},
            {"69639df789022856", "Google Chrome 86.0.4240.111"},
            {"6a316aa67a46820b", "Core FTP LE 1.3c (Build 1437) / 2.2 (Build 1689)"},
            {"6a8b377d0f5cb666", "WinSCP 2.3.0 (Build 146)"},
            {"6aa18a60024620ae", "GCN 2.9.1"},
            {"6b3a5ce7ad4af9e4", "IceChat 9 RC2"},
            {"6bb54d82fa42128d", "WinSCP 4.3.4 (Build 1428)"},
            {"6bb98fb8cdc26d69", "Calculator (Windows built-in)"},
            {"6bc3383cb68a3e37", "iTunes 7.6.0.29 / 8.0.0.35"},
            {"6d2bac8f1edf6668", "Microsoft Office Outlook"},
            {"6e855c85de07bc6a", "Microsoft Office Excel 2010 x64"},
            {"6e9a79992da9ea2", "Nokia PC Suite 7.1"},
            {"6e9d40a4c63bb562", "Real Player Alternative 1.25 (Media Player Classic 6.4.8.2 / 6.4.9.0)"},
            {"6f647f9488d7a", "AIM 7.5.11.9 (custom AppID + JL support)"},
            {"6fee01bd55a634fe", "Smuxi 0.8.0.0"},
            {"7010c278903c2b0f", "Adobe Acrobat XI Pro 32-bit"},
            {"70b52cf73249257", "Sococo 1.5.0.2274"},
            {"70d9ada92108d731", "IrfanView 4.51 (x64)"},
            {"70ffd305907c983b", "7zip 18.05"},
            {"7111c0ce965b7246", "Battle.net"},
            {"714b179e552596df", "Bullet Proof FTP 2.4.0 (Build 31)"},
            {"7192f2de78fd9e96", "TIFNY 5.0.3"},
            {"728008617bc3e34b", "eM Client 3.0.10206.0"},
            {"73c6a317412687c2", "Google Talk 1.0.0.104"},
            {"73ce3745a843c0a4", "FrostWire 5.1.4"},
            {"7494a606a9eef18e", "Crystal Player 1.98"},
            {"74d7f43c1561fc1e", "Windows Media Player 12"},
            {"74ea779831912e30", "Skype 7.24.0.104"},
            {"7526de4a8b5914d9", "Forte Agent 6.00 (Build 32.1186)"},
            {"7593af37134fd767", "RealPlayer 6.0.6.99 / 7 / 8 / 10.5"},
            {"75fdacd8330bac18", "AnyDesk"},
            {"76689ff502a1fd9e", "Imagine Image and Animation Viewer 1.0.7"},
            {"76f6f1bd18c19698", "aMule 2.2.6"},
            {"776beb1fcfc6dfa5", "Thunderbird 1.0.6 (20050716) / 3.0.2"},
            {"777483d3cdac1727", "Gajim 0.14.4"},
            {"780732558f827a42", "AutoPix 5.3.3"},
            {"784182360de0c5b6", "Kazaa Lite 1.7.1"},
            {"78f0afb5bd4bb278", "Microsoft Lync 2016 64-bit (Skype for Business)"},
            {"7904145af324576e", "Total Commander 7"},
            {"792699a1373f1386", "Piolet 3.1.1"},
            {"79370f660ab51725", "UploadFTP 2.0.1.0"},
            {"7937df3c65790919", "FTP Explorer 10.5.19 (Build 001)"},
            {"7a4ba998575ff2a4", "FreeCommander XE 2016 Build 715 32-bit"},
            {"7a7c60efd66817a2", "Spotnet 1.7.4"},
            {"7a8db574299c8568", "Windows Movie Maker 2012 (build 16.4.3528.0331)"},
            {"7b2b4f995b54387d", "News Reactor 20100224.16"},
            {"7b4d500e147e4391", "Tor Browser 8.0.4 (x64)"},
            {"7b7f65aaeca20a8c", "Dropbox App 5.4.24"},
            {"7c2916afd6f116a6", "LibreOffice 5.1.0.3 Base"},
            {"7cb0735d45243070", "CDisplay 1.8.1.0"},
            {"7dca40fd2a5a971f", "LibreOffice 5.1.0.3"},
            {"7e4dca80246863e3", "Control Panel"},
            {"7fd04185af357bd5", "UltraLeeacher 1.7.0.2969 / 1.8 Beta (Build 3490)"},
            {"8172865a9d5185cb", "Binreader 1.0 (Beta 1)"},
            {"817bb211c92fd254", "GOM Player 2.0.12.3375 / 2.1.28.5039"},
            {"817e5ad5be351574", "Microsoft Built-in Services 10.0.10011.16384 (Win10)"},
            {"8211531a7918b389", "Newsbin Pro 6.00 (Build 1019) (JL support)"},
            {"83b03b46dcd30a0e", "iTunes "},
            {"83dd64e7fa560bd5", "LibreOffice 5.1.0.3 Calc"},
            {"84f066768a22cc4f", "Adobe Photoshop CS5 (64-bit)"},
            {"8628e76fd9020e81", "Fling File Transfer Plus 2.24"},
            {"86781fe8437db23e", "Messenger Pro 2.66.6.3353"},
            {"86b804f7a28a3c17", "Miranda IM 0.6.8 / 0.7.6 / 0.8.27 / 0.9.9 / 0.9.29"},
            {"884fd37e05659f3a", "VZOchat 6.3.5"},
            {"888f2fa044591eda", "Twitter - Trusted Microsoft Store App (Win10)"},
            {"8904a5fd2d98b546", "IceChat 7.70 20101031"},
            {"89b0d939f117f75c", "Adobe Acrobat 9 Pro Extended (32-bit)"},
            {"8a1c1c7c389a5320", "Safari 3.2.3 (525.29)"},
            {"8a461f82e9eb4102", "Foxit Reader 7.2.0.722"},
            {"8b87640a40ec9fc", "Snagit 2020"},
            {"8bce06a9e923e1f9", "Slack 4.10.3"},
            {"8bd5c6433ca967e9", "ACDSee Photo Manager 2009 (v11.0 Build 113)"},
            {"8c816c711d66a6b5", "MSN Messenger 6.2.0137 / 7.0.0820"},
            {"8dcca8b24a5e822e", "CDBurnerXP 4.5.7.6623"},
            {"8deb27dfa31c5c2a", "CoffeeCup Free FTP 4.4 (Build 1904)"},
            {"8e4e81d9adc545b8", "Microsoft Your Phone"},
            {"8eafbd04ec8631ce", "VMware Workstation "},
            {"8f3d7202aa5d4c01", "ImgBurn 2.5.8.0"},
            {"8f852307189803b8", "Far Manager 2.0.1807"},
            {"8fb5ce5e2b049ce", "Windows Defender (Win10 built-in)"},
            {"8fd1364019dc2115", "Calibre E-Book Manager 2.33"},
            {"8fdb062f1e486cac", "Microsoft Powerpoint 2013 32-bit"},
            {"9027fe24326910d2", "Thunderbird 38.6.0"},
            {"905c98e216107aa1", "Microsoft Lync 2013 15.0.4753.1000"},
            {"9077b9c9cf187cc2", "KeePass 1.36"},
            {"90e5e8b21d7e7924", "Winamp 3.0d (Build 488)"},
            {"918e0ecb43d17e23", "Notepad (32-bit)"},
            {"92f1d5db021cd876", "NewsLeecher 4.0 / 5.0 Beta 6"},
            {"9390ee5b658e96e", "PuTTY 0.72 / 0.73"},
            {"939c10c2c101c1b0", "Stickies 9.0d"},
            {"93b18adf1d948fa3", "qutIM 0.2"},
            {"954ea5f70258b502", "Windows Script Host - wscript.exe (32-bit)"},
            {"9560577fd87cf573", "LeechFTP 1.3 (Build 207)"},
            {"959668a81d4f220e", "Sublime Text 3.2.1 (Build 3207)"},
            {"96252daff039437a", "Lphant 7.0.0.112351"},
            {"966fa7c312d9b10", "Eraser 6.2.0.2970"},
            {"969252ce11249fdd", "Mozilla Firefox 40.0 / 44.0.2"},
            {"9749cea96d411f37", "HexChat 2.10.2 64-bit"},
            {"977a5d147aa093f4", "Lphant 3.51"},
            {"9839aec31243a928", "Microsoft Office Excel 2010 x86"},
            {"989d7545c2b2e7b2", "IMVU 465.8.0.0"},
            {"98b0ef1c84088", "fulDC 6.78"},
            {"99c15cf3e6d52b61", "mldonkey 3.1.0"},
            {"9a165f62edbfa161", "Microsoft Store"},
            {"9a3bdae86d5576ee", "WinSCP 3.2.1 (Build 174) / 3.8.0 (Build 312)"},
            {"9a464053cd82de6d", "LINE Messenger"},
            {"9ad1ec169bf2da7f", "FlylinkDC++ r405 (Build 7358)"},
            {"9ad84c52efeae190", "1Password 4.6.0.604"},
            {"9b9cdc69c1c24e2b", "Notepad 64-bit"},
            {"9c08ad74ad8708df", "Microsoft Publisher 2016 64-bit"},
            {"9c32e2313792e6e8", "Microsoft Built-in Disk Cleanup (Win10)"},
            {"9c7cc110ff56d1bd", "Microsoft Office PowerPoint 2010 x86"},
            {"9ce6555426f54b46", "HxD 1.7.7.0"},
            {"9d1f905ce5044aee", "Edge Browser"},
            {"9d78513a8998829c", "Microsoft Built-in Run Dialog (Win7 + Win10)"},
            {"9d91276b0be3e46b", "Windows Help and Support (Built-in) Win7"},
            {"9dacebaa9ac8ca4e", "TLNews Newsreader 2.2.0 (Build 2430)"},
            {"9e0b3f677a26bbc4", "BitKinex 3.2.3"},
            {"9edafe4ba4b22ce7", "Eclipse IDE Oxygen (4.7.3a)"},
            {"9eff0b23d51fe003", "XMind 201807140020"},
            {"9f03ae476ad461fa", "GroupsAloud 1.0"},
            {"9f5c7755804b850a", "Windows Script Host - wscript.exe (64-bit)"},
            {"9fda41b86ddcf1db", "VLC Media Player"},
            {"9fdb10e18cdd0101", "Cisco AnyConnect Secure Mobility Client 3.1.02040"},
            {"a028c9db28aa15a3", "Piriform Defraggler 2.20.989 64-bit"},
            {"a0d6b1b874c6e9d2", "TOR Browser 6.0.2"},
            {"a10b45adb36c1d27", "PST Walker 5.54"},
            {"a18df73203b0340e", "Microsoft Word 2016"},
            {"a1d19afe5a80f80", "FileZilla 2.2.32"},
            {"a2b95ca27b6c33d9", "Windows Live Photo Gallery"},
            {"a2c73c383525f1bb", "RealVNC Viewer 5.3.0 64-bit"},
            {"a31ec95fdd5f350f", "BitComet 0.49 / 0.59 / 0.69 / 0.79 / 0.89 / 0.99 / 1.07 / 1.28"},
            {"a3e0d98f5653b539", "Instantbird 1.0 (20110623121653) (JL support)"},
            {"a4a5324453625195", "Microsoft Word 2013 32-bit"},
            {"a4def57ee99d77e9", "Nomad News 1.43"},
            {"a52b0784bd667468", "Photos Microsoft 16.526.11220.0 (Windows 10)"},
            {"a55ed4fbb973aefb", "Microsoft Teams"},
            {"a581b8002a6eb671", "WiseFTP 5.5.9"},
            {"a5db18f617e28a51", "ICQ 6.5 (Build 2024)"},
            {"a6d4dfec09c69409", "Microsoft Word Viewer 11.8169.8172"},
            {"a746f9625f7695e8", "HeXHub 5.07"},
            {"a75b276f6e72cf2a", "Kazaa Lite Tools K++ 2.7.0"},
            {"a777ad264b54abab", "JetVideo 8.0.2.200 Basic"},
            {"a79a7ce3c45d781", "CuteFTP 7.1 (Build 06.06.2005.1)"},
            {"a7ba40025dac9a67", "Microsoft Office Hub"},
            {"a7bd71699cd38d1c", "Microsoft Office Word 2010 x86"},
            {"a8c43ef36da523b1", "Microsoft Office Word 2003 Pinned and Recent."},
            {"a8df13a46d66f6b5", "Kommute (Calypso) 0.24"},
            {"aa11f575087b3bdc", "Unzbin 2.6.8"},
            {"ac3a63b839ac9d3a", "Vuze 4.6.0.4"},
            {"ac8920ed05001800", @"@DMDirc 0.6.5 (Profile store: C:\Users\$user\AppData\Roaming\DMDirc\)"},
            {"accca100973ef8dc", "Azureus 2.0.8.4"},
            {"ace8715529916d31", "40tude Dialog 2.0.15.1 (Beta 38)"},
            {"adecfb853d77462a", "Microsoft Office Word 2007 Pinned and Recent."},
            {"ae069d21df1c57df", "mIRC 6.35 / 7.19"},
            {"ae3f2acd395b622e", "QuickTime Player 6.5.1 / 7.0.3 / 7.5.5 (Build 249.13)"},
            {"ae6df75df512bd06", "Microsoft Groove Music (Build 10.19031.1141.0)"},
            {"aedd2de3901a77f4", "Pidgin 2.0.0 / 2.10.0 / 2.7.3"},
            {"af0fdd562e3f275b", "Snagit 2020"},
            {"b0236d03c0627ac4", "ICQ 5.1 / ICQLite Build 1068"},
            {"b0459de4674aab56", "Windows Virtual PC - vmwindow.exe (32- and 64-bit)"},
            {"b06a975b62567622", "Windows Live Messenger 8.5.1235.0517 BETA"},
            {"b08971c77377bde3", "Microsoft Visual Studio Community 2015"},
            {"b17d3d0c9ca7e29", "Picasa 3.8.0 (build 117.43, 0) / 3.9.141 (build 259)"},
            {"b223c3ffbc0a7a42", "Bersirc 2.2.14"},
            {"b3016b8da2077262", "eMule 0.50a"},
            {"b3965c840bf28ef4", "AIM 4.8.2616"},
            {"b39bc6b590f53961", "HexChat 2.10.2 32-bit"},
            {"b39c5f226977725d", "ACDSee Pro 8.1.99"},
            {"b3f13480c2785ae", "Paint 6.1 (build 7601: SP1)"},
            {"b48ce76eda60b97", "Shareaza 8.0.0.112300"},
            {"b50ee40805bd280f", "QuickTime Alternative 1.9.5 (Media Player Classic 6.4.9.1)"},
            {"b6267f3fcb700b60", "WiseFTP 4.1.0"},
            {"b7173093b23b9a6a", "Beyond Compare 4"},
            {"b74736c2bd8cc8a5", "WinZip"},
            {"b77ef7f3fc946302", "Pale Moon Browser 26.1.1 (32-bit)"},
            {"b7cb1d1c1991accf", "FlashFXP 4.0.0 (Build 1548)"},
            {"b868d9201b866d96", "Microsoft Lync 4.0.7577.0"},
            {"b8ab77100df80ab2", "Microsoft Excel 2016 64-bit"},
            {"b8c13a5dd8c455a2", "Titan FTP Server 8.40 (Build 1338)"},
            {"b8c29862d9f95832", "Microsoft Office InfoPath 2010 x86"},
            {"b91050d8b077a4e8", "Windows Media Center (Win7)"},
            {"ba132e702c0147ef", "KCeasy 0.19-rc1"},
            {"ba3a45f7fd2583e1", "Blubster 3.1.1"},
            {"baacb5294867b833", "Notepad++ 7.8.6"},
            {"bac8a6b507360131", "Remote Desktop Connection Manager 2.2"},
            {"baea31eacd87186b", "BinaryBoy 1.97 (Build 55)"},
            {"bba8a4896f0d26f", "Ares Chat Client (3.1.9.4045)"},
            {"bc03160ee1a59fc1", "Foxit PDF Reader 5.4.5"},
            {"bc0c37e84e063727", "Windows Command Processor - cmd.exe (32-bit)"},
            {"bc2f88eccd3461b4", "Microsoft Built-in Event Viewer 1.0 (Win10)"},
            {"bcc705f705d8132b", "Instan-t 5.2 (Build 2824)"},
            {"bcd7ba75303acbcf", "BitLord 1.1"},
            {"bd050ac447f6cd65", "Microsoft Xbox App"},
            {"bd249197a6faeff2", "Windows Live Messenger 2011"},
            {"be4875bb3e0c158f", "CrossFTP 1.75a"},
            {"be71009ff8bb02a2", "Microsoft Office Outlook x86"},
            {"bec10d3aaf939ffa", "Pale Moon Browser 26.1.1 (64-bit)"},
            {"bf483b423ebbd327", "Binary Vortex 5.0"},
            {"bf9ae1f46bd9c491", "Nimbuzz 2.0.0 (rev 6266)"},
            {"bfc1d76f16fa778f", "Ares (Galaxy) 1.8.4 / 1.9.8 / 2.1.0 / 2.1.7.3041"},
            {"bfe841f4d35c92b1", "QuadSucker/News 5.0"},
            {"c01827d56ff89056", "Microsoft Sticky Notes"},
            {"c01d68e40226892b", "ClicksAndWhistles 2.7.146"},
            {"c02baf50d02056fc", "FotoVac 1.0"},
            {"c04f69101c131440", "CuteFTP 5.0 (Build 50.6.10.2)"},
            {"c1eece5026414c64", "Recuva 1.52.1086 (64-bit)"},
            {"c2d349a0e756411b", "Adobe Reader 8.1.2"},
            {"c312e260e424ae76", "Mail.Ru Agent 5.8 (JL support)"},
            {"c31b3d36438b5e2c", "Visual Studio Enterprise 2017 Version 15.9.10"},
            {"c5236fd5824c9545", "PLAYXPERT 1.0.140.2822"},
            {"c54b96f328bdc28d", "WiseFTP 7.3.0"},
            {"c5c24a503b1727df", "XnView 1.98.2 Small / 1.98.2 Standard"},
            {"c5ef839d8d1c76f4", "LimeWire 5.2.13"},
            {"c634153e7f5fce9c", "IrfanView 3.10 / 4.30"},
            {"c6f7b5bf1b9675e4", "BitWise IM 1.7.3a"},
            {"c71ef2c372d322d7", "PGP Desktop 10"},
            {"c765823d986857ba", "Adobe Illustrator CS5 (32-bit)"},
            {"c7a4093872176c74", "Paint Shop Pro Pinned and Recent."},
            {"c8112ac53c5ed250", "Jetico Log Viewer 1.1"},
            {"c845f3a6022d647c", "Another File 2.03 (Build 2/7/2004)"},
            {"c8aa3eaee3d4343d", "Trillian 0.74 / 3.1 / 4.2.0.25 / 5.0.0.35 (JL support)"},
            {"c8e4c10e5460b00c", "iMesh 6.5.0.16898"},
            {"c91d08dcfc39a506", "SM Player 0.6.9 r3447"},
            {"c9374251edb4c1a8", "BitTornado T-0.3.17"},
            {"c98ab5ccf25dda79", "NewsShark 2.0"},
            {"c9950c443027c765", "WinZip 9.0 SR-1 (6224) / 10.0 (6667)"},
            {"c997d2e1a0f0929", "BCWipe 6.08.6"},
            {"c99ddde925d26df3", "Robo-FTP 3.7.9 CronMaker"},
            {"ca1eb46544793057", "RetroShare 0.5.2a (Build 4550)"},
            {"ca942805559495e9", "aMSN 0.98.4"},
            {"caea34d2e74f5c8", "uTorrent 3.4.7"},
            {"cb1d97aca3fb7e6b", "Newz Crawler 1.9.0 (Build 4100)"},
            {"cb5250eaef7e3213", "ApexDC++ 1.4.3.957"},
            {"cb984e3bc7faf234", "NewsRover 17.0 (Rev.0)"},
            {"cb996a858d7f15c", "PDF Architect 4.0.09.25450 64-bit"},
            {"cbbe886eca4bfc2d", "ExoSee 1.0.0"},
            {"cbeb786f0132005d", "VLC 0.7.2"},
            {"cc4b36fbfb69a757", "gtk-gnutella 0.97"},
            {"cc76755e0f925ce6", "AllPicturez 1.2"},
            {"cca6383a507bac64", "Gadu-Gadu 10.5.2.13164"},
            {"ccb36ff8a8c03b4b", "Azureus 2.5.0.4 / Vuze 3.0.5.0"},
            {"ccba5a5986c77e43", "Microsoft Edge (Chromium)"},
            {"ccc0fa1b9f86f7b3", "CCleaner 5.15.5513 64-bit"},
            {"cd2acd4089508507", "AbsoluteTelnet 9.18 Lite"},
            {"cd40ead0b1eb15ab", "NNTPGrab 0.6.2"},
            {"cd8cafb0fb6afdab", "uTorrent 1.7.7 (Build 8179) / 1.8.5 / 2.0 / 2.21 (Build 25113) / 3.0 (Build 25583)"},
            {"cdb6f0c373f2da0f", "stunnel 5.31"},
            {"cdf30b95c55fd785", "Microsoft Office Excel 2007"},
            {"cf6379a9a987366e", "Digibin 1.31"},
            {"cfab0ec14b6f953", "Express NewsPictures 2.41 (Build 08.05.07.0)"},
            {"cfb56c56fa0f0a54", "Mozilla 0.9.9"},
            {"d00655d2aa12ff6d", "Microsoft Office PowerPoint x64"},
            {"d0261ed6e16b200b", "News File Grabber 4.6.0.4"},
            {"d1fc019238236806", "Newsgroup Commander Pro 9.05"},
            {"d22ad6d9d20e6857", "ALLPlayer 4.7"},
            {"d249d9ddd424b688", "Google Chrome 81.0.4044.138"},
            {"d28ee773b2cea9b2", "3D-FTP 9.0 build 7"},
            {"d2d0fc95675fb2c8", "Microsoft Built-in Print Management (Win10)"},
            {"d33ecf70f0b74a77", "Picasa 2.2.0 (Build 28.08, 0)"},
            {"d3530c5294441522", "HydraIRC 0.3.165"},
            {"d356105fac5527ef", "Steam 1/22/2021"},
            {"d38a3ea7ec79fbed", "LibreOffice 5.1.0.3 Writer"},
            {"d38adec6953449ba", "Microsoft Office OneNote 2010 x64"},
            {"d3c5cf21e86b28af", "SeaMonkey 2.3.3"},
            {"d41746b133d17456", "Tkabber 0.11.1"},
            {"d460280b17628695", "Java Binary"},
            {"d4a589cab4f573f7", "Microsoft Project 2010 x86"},
            {"d53b52fb65bde78c", "Android Newsgroup Downloader 6.2"},
            {"d5c02fc7afbb3fd4", "NNTPGrab 0.6.2 Server"},
            {"d5c3931caad5f793", "Adobe Soundbooth CS5 (32-bit)"},
            {"d64d36b238c843a3", "Microsoft Office InfoPath 2010 x86"},
            {"d7528034b5bd6f28", "Windows Live Mail Pinned and Recent."},
            {"d7666c416cba240c", "NewsMan Pro 3.0.5.2"},
            {"d78150e0484a4e1d", "Evernote 5.9.6.9494"},
            {"d7d647c92cd5d1e6", "uTalk 2.6.4 r47692"},
            {"d7db75db9cdd7c5d", "Xnews 5.04.25"},
            {"d8081f151f4bd8a5", "CuteFTP 8.3 Lite (Build 8.3.4.0007)"},
            {"d838aac097abece7", "ACDSee Photo Manager 12 (Build 344)"},
            {"d8671c1ed93c75c8", "Tor Browser 5.5.2"},
            {"d93f411851d7c929", "Windows Powershell 5.0 32-bit"},
            {"d97efdf3888fe7eb", "KeePass 2.31"},
            {"da7e8de5b8273a0f", "Yahoo Messenger 5.0.0.1226 / 6.0.0.1922"},
            {"db3b8d985f0668e", "FreeFileSync 10.7"},
            {"dba909a61476ccec", "NewsWolf 1.41"},
            {"dc64de6c91c18300", "Brosix Communicator 3.1.3 (Build 110719 nid 1)"},
            {"dd658a07478b46c2", "PIRCH98 1.0.1.1190"},
            {"dd7c3b1adb1c168b", "Microsoft Game Bar"},
            {"de48a32edcbe79e4", "Acrobat Reader 15.x"},
            {"de76415e0060ce13", "Noworyta News Reader 2.9"},
            {"dee18f19c7e3a2ec", "PopNote 5.21"},
            {"e0246018261a9ccc", "qutIM 0.2.80.0"},
            {"e0532b20aa26a0c9", "QQ International 1.1 (2042)"},
            {"e0f7a40340179171", "imule 1.4.5 (rev. 749)"},
            {"e107946bb682ce47", "FileZilla 3.5.1"},
            {"e1d47cb031dafb9f", "BearShare 6.0.0.22717 / 8.1.0.70928 / 10.0.0.112380"},
            {"e26f61afb0824f2e", "Photoshop CC 2015"},
            {"e2a593822e01aed3", "Adobe Flash CS5 (32-bit)"},
            {"e30bbea3e1642660", "Neebly 1.0.4"},
            {"e31a6a8a7506f733", "Image AXS Pro 4.1"},
            {"e36bfc8972e5ab1d", "XPS Viewer"},
            {"e40cb5a291ad1a5b", "Songbird 1.9.3 (Build 1959)"},
            {"e42a8e0f4d9b8dcf", "Sysax FTP Automation 5.15"},
            {"e4bd2558bfab368d", "UltraDefrag 7.0.0"},
            {"e57cfc995bdc1d98", "Snagit 11"},
            {"e6ea77a1d4553872", "Gnucleus 1.8.6.0"},
            {"e6ee34ac9913c0a9", "VLC 0.6.2"},
            {"e6ef42224b845020", "ALFTP 5.20.0.4"},
            {"e70d383b15687e37", "Notepad++ 5.6.8 (32-bit)"},
            {"e73d9f534ed5618a", "BitSpirit 1.2.0.228 / 2.0 / 2.6.3.168 / 2.7.2.239 / 2.8.0.072 / 3.1.0.077 / 3.6.0.550"},
            {"e76a4ef13fbf2bb1", "Manolito 3.1.1"},
            {"e93dbdcede8623f2", "Pandion 2.6.106"},
            {"e9a39dfba105ea23", "FastStone Image Viewer 4.6"},
            {"ea83017cdd24374d", "IrfanView Thumbnails"},
            {"eab25958dbddbaa4", "Binary News Reaper 2 (Beta 0.14.7.448)"},
            {"eb3300e672136bc7", "Stream Reactor 1.0 Beta 9 (uses VLC!)"},
            {"eb7e629258d326a1", "WindowWasher 6.6.1.18"},
            {"ebd8c95d87f25154", "Carrier 2.5.5"},
            {"ec3e36af0cdcb3e1", "Steam build 2/4/2016"},
            {"ecd1a5e2c3af9c46", "LibreOffice 5.1.0.3 Press"},
            {"ecd21b58c2f65a2f", "StealthNet 0.8.7.9"},
            {"ecdd9154e84d5544", "Wickr Top Secret Messenger Desktop 2.3.5"},
            {"ed49e1e6ccdba2f5", "GNUnet 0.8.1a"},
            {"ed7a5cc3cca8d52a", "CCleaner 1.32.345 / 1.41.544 / 2.36.1233 / 3.10.1525"},
            {"edc786643819316c", "HoneyView3 #5834"},
            {"ee0c103672a7a2b9", "ManyCam 6.7.0"},
            {"ee462c3b81abb6f6", "Adobe Reader X 10.1.0"},
            {"ef473fab8120b354", "uTorrent 3.5.5"},
            {"ef606b196796ebb", "HP MediaSmart Photo"},
            {"efb08d4e11e21ece", "Paltalk Messenger 10.0 (Build 409)"},
            {"efbb2bf3c1d06466", "Auslogics Disk Defrag 6.2.1.0"},
            {"f001ea668c0aa916", "Cabos 0.8.2"},
            {"f01b4d95cf55d32a", "Windows Explorer Windows 8.1."},
            {"f0275e8685d95486", "Microsoft Excel 2013 32-bit"},
            {"f0468ce1ae57883d", "Adobe Reader 7.1.0"},
            {"f09b920bfb781142", "Camfrog 4.0.47 / 5.5.0 / 6.1 (build 146) (JL support)"},
            {"f0c7bd3e0584a65a", "InfraRecorder 0.53.0.0 32-bit"},
            {"f18460fded109990", "Windows Connected Devices"},
            {"f1a4c04eebef2906", "[i2p] Robert 0.0.29 Preferences"},
            {"f214ca2dd40c59c1", "FrostWire 4.20.9"},
            {"f2cb1c38ab948f58", "X-Chat 1.8.10 / 2.6.9 / 2.8.9"},
            {"f5ac5390b9115fdb", "Microsoft Office PowerPoint 2007"},
            {"f5e4e50707bcd215", "Microsoft Message Analyzer 1.4"},
            {"f61b65550a84027e", "iMesh 11.0.0.112351"},
            {"f64de962764b9b0f", "FTPRush 1.1.3 / 2.15"},
            {"f674c3a77cfe39d0", "Winamp 2.95 / 5.1 / 5.621"},
            {"f6fd5d99e2b6e178", "LibreOffice 5.1.0.3 Draw"},
            {"f784591ff7f60f76", "Microsoft Built-in Defragment and Optimize Drives (Win10)"},
            {"f82607a219af2999", "Cyberduck 4.1.2 (Build 8999)"},
            {"f91fd0c57c4fe449", "ExpanDrive 2.1.0"},
            {"f920768fe275f7f4", "Grabit 1.5.3 Beta (Build 909) / 1.6.2 (Build 940) / 1.7.2 Beta 4 (Build 997)"},
            {"f92e607f9de02413", "RealPlayer 14.0.6.666"},
            {"fa02aa2c575837a6", "Microsoft Built-in Task Scheduler 1.0 (Win10)"},
            {"fa496fe13dd62edf", "KVIrc 3.4.2.1 / 4.0.4"},
            {"fa7144034d7d083d", "Directory Opus 10.0.2.0.4269 (JL tasks supported)"},
            {"fac3aa4105c6c466", "Microsoft Built-in System Restore (Win7)"},
            {"faef7def55a1d4b", "VLC 2.2.6"},
            {"fb1f39d1f230480a", "Bopup Messenger 5.6.2.9178 (all languages: en,du,fr,ger,rus,es)"},
            {"fb230a9fe81e71a8", "Yahoo Messenger 11.0.0.2014-us"},
            {"fb3b0dbfee58fac8", "Microsoft Word 2016 64-bit"},
            {"fb7ca8059b8f2123", "ooVoo 3.0.7.21"},
            {"fc999f29bc5c3560", "Robo-FTP 3.7.9"},
            {"fd1ad55e472f20e0", "Google Earth Pro 7.3.2.5491"},
            {"fdbaca0a1fce6055", "MozBackup 1.5.1"},
            {"fdbd48d45512dffc", "Photoshop 7"},
            {"fe57f5df17b45fe", "Wireshark 2.6.3"},
            {"fe5e840511621941", "JetAudio 5.1.9.3018 Basic / 6.2.5.8220 Basic / 7.0.0 Basic / 8.0.16.2000 Basic"},
            {"fe8bb4692de7b989", "Smart Defrag 4.3.0.847"},
            {"fe9e0f7260000a12", "RealVNC Server 5.3.0 64-bit (Connect+File Transfer)"},
            {"ff103e2cc310d0d", "Adobe Reader XI"},
            {"ff224628f0e8103c", "Morpheus 3.0.3.6"},
            {"ff99ba2fb2e34b73", "Microsoft Windows Calculator"},
        };
        #endregion

        public StructuredStorageFilter(List<string> masks) {
            _masks = masks;
        }

        // TODO: Not a great way to check, look into decoding the actual binary content into a useful format.
        private bool ContainsSubstring(byte[] bytes, string s) {
            for (int i = 0; i < bytes.Length - s.Length; i++) {
                bool match = true;
                

                int offset = 0;
                foreach (var c in s) {
                    if (char.ToLower((char)bytes[i + offset]) != char.ToLower(c)) {
                        match = false;
                        break;
                    }

                    offset++;
                }

                if (match) {
                    return true;
                }
            }

            return false;
        }

        private int GetFilenameEndIdx(byte[] bytes, int offset) {
            byte[] desired = new byte[] { 0x00, 0x00 };
            for (int i = offset; i < bytes.Length - desired.Length; i++) {
                if (bytes[i] == desired[0] && bytes[i + 1] == desired[1]) {
                    return i;
                }
            }

            return -1;
        }

        private string GetLnkTarget(byte[] linkBytes) {
            var tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".lnk");
            try {
                try {
                    File.WriteAllBytes(tmp, linkBytes);
                    try {
                        // TODO: Verify that we're dealing with a .lnk before writing this to disk etc?
                        return LinkUtil.GetLnkTarget(_shell, tmp);
                    }
                    catch (System.Runtime.InteropServices.COMException) {
                        return string.Empty; // Not a .lnk
                    }
                }
                catch (IOException ex) {
                    Logger.Warn(ex);
                    return string.Empty;
                }
            }
            finally {
                try {
                    File.Delete(tmp);
                }
                catch (Exception ex) {
                    Logger.Warn(ex);
                }
            }
        }

        private bool ContainsPattern(CFStorage storage, string fileName) {
            var stream = storage.GetStream(fileName);
            var data = stream.GetData();
            if (data.Length == 0)
                return false;

            foreach (var pattern in _masks) {

                var target = GetLnkTarget(data);
                if (target.ToLowerInvariant().StartsWith(pattern.ToLowerInvariant())) {
                    return true;
                }
                
                /*if (ContainsSubstring(data, pattern)) {
                    return true;
                }*/
            }

            return false;
        }

        private static string GetApplicationName(string filename) {
            var nameWithoutExt = Path.GetFileNameWithoutExtension(filename);
            var type = ApplicationMapping.ContainsKey(nameWithoutExt) ? ApplicationMapping[nameWithoutExt] : "Unknown";
            return type;
        }

        private void HandleFile(FileStream fs, string fileName) {
            var type = GetApplicationName(fileName);

            using (var cf = new CompoundFile(fs, CFSUpdateMode.Update, CFSConfiguration.SectorRecycle | CFSConfiguration.NoValidationException | CFSConfiguration.EraseFreeSectors)) {
                CFStorage storage = cf.RootStorage;

                int numDeleted = 0;
                Action<CFItem> va = delegate (CFItem target) {
                    bool doFilter = ContainsPattern(storage, target.Name);
                    bool canFilter = !"DestList".Equals(target.Name);

                    if (doFilter) {
                        if (canFilter) {
                            Console.WriteLine($"Deleting {target.Name} from {fileName} {type}");
                            storage.Delete(target.Name);
                            numDeleted++;
                        }
                        else {
                            Console.WriteLine($"Found but skipping {target.Name} from {fileName}");
                        }
                    }

                };
                storage.VisitEntries(va, false);

                if (numDeleted > 0) {
                    cf.Commit();
                    Logger.Debug($"Modifying {fileName}, deleted {numDeleted} entries");
                }
            }
        }

        public void Purge() {
            if (_masks.Count == 0)
                return;

            var path = Path.Combine(
                Environment.GetEnvironmentVariable("AppData"),
                "Microsoft",
                "Windows",
                "Recent",
                "AutomaticDestinations"
            );

            //var files = Directory.EnumerateFiles(path, "*.automaticDestinations-ms");
            var files = new string[] { Path.Combine(path, "5f7b5f1e01b83767.automaticDestinations-ms") };
            foreach (var fileName in files) {
                try {
                    using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite)) {
                        HandleFile(fs, fileName);
                    }
                }
                catch (System.IO.IOException ex) {
                    Logger.Warn(ex);
                }
            }
        }

        public void SetMasks(List<string> masks) {
            _masks = masks;
        }

    }
}
