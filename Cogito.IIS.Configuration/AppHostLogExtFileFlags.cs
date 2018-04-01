using System;

namespace Cogito.IIS.Configuration
{

    [Flags]
    public enum AppHostLogExtFileFlags
    {

        Date = 1,
        Time = 2,
        ClientIP = 4,
        UserName = 8,
        SiteName = 16,
        ComputerName = 32,
        ServerIP = 64,
        Method = 128,
        UriStem = 256,
        UriQuery = 512,
        HttpStatus = 1024,
        Win32Status = 2048,
        BytesSent = 4096,
        BytesRecv = 8192,
        TimeTaken = 16384,
        ServerPort = 32768,
        UserAgent = 65536,
        Cookie = 131072,
        Referer = 262144,
        ProtocolVersion = 524288,
        Host = 1048576,
        HttpSubStatus = 2097152,

    }

}