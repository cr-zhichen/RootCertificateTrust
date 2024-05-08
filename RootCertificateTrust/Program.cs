using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

try
{
    // 获取程序的执行路径
    string? exePath = Path.GetDirectoryName(AppContext.BaseDirectory);

    // 检查路径是否为空
    if (string.IsNullOrEmpty(exePath))
    {
        Console.WriteLine("无法获取执行路径，程序将使用默认工作目录。");
    }
    else
    {
        // 设置当前工作目录为程序的执行路径
        Directory.SetCurrentDirectory(exePath);
    }

    // 证书的PEM文件路径
    string pemPath = args.Length > 0 ? args[0] : "pem\\root.pem";

    // 读取PEM文件内容
    string pemContents = File.ReadAllText(pemPath);

    // 创建一个集合来存储证书
    var collection = new X509Certificate2Collection();
    collection.ImportFromPem(pemContents);

    // 获取根证书的存储区
    using var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
    store.Open(OpenFlags.ReadWrite);
    foreach (var cert in collection)
    {
        // 将每个证书添加到存储中
        store.Add(cert);
        Console.WriteLine($"已将 {cert.Subject} 添加到根存储。");
    }
    store.Close();
    
    // 启动证书管理器
    Console.WriteLine("打开根证书管理界面...");
    Process.Start("mmc.exe", "certmgr.msc");
}
catch (Exception ex)
{
    Console.WriteLine($"发生错误：{ex.Message}");
}

Console.WriteLine("按下任意键退出..");
Console.ReadKey();
