private bool IsSocketConnected(Socket socket)
{
    try
    {
        if (socket == null || !socket.Connected)
        {
            return false; // 소켓이 null이거나 연결되지 않았으면 false
        }

        // Poll 메서드를 사용해 소켓 상태 확인
        if (socket.Poll(1000, SelectMode.SelectRead) && socket.Available == 0)
        {
            // 소켓이 읽기 가능하며 데이터가 없다면 연결이 끊어진 것으로 간주
            return false;
        }

        return true; // 연결 상태가 정상
    }
    catch
    {
        return false; // 예외 발생 시 연결이 끊어진 것으로 간주
    }
}
