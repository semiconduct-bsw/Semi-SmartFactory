private void LogMessage(string message)
{
    // 날짜별로 로그 파일 경로 지정
    string logFilePath = $"20241112-CIMPCGUI01\\log\\{DateTime.Now:yyyyMMdd}.log";
    Directory.CreateDirectory(Path.GetDirectoryName(logFilePath)); // 경로가 없을 시 자동 생성

    // 로그 메시지에 타임스탬프 추가
    string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

    // 파일에 로그 메시지 추가
    File.AppendAllText(logFilePath, logEntry + Environment.NewLine);

    // LogForm의 LogBox에 로그 추가
    if (LogFormInstance != null && !LogFormInstance.IsDisposed)
    {
        LogFormInstance.AppendLog(logEntry);
    }
}
