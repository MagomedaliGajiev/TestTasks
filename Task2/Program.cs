public static class Server
{
    private static int count = 0;
    private static readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();

    public static int GetCount()
    {
        rwLock.EnterReadLock();
        try
        {
            return count;
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }

    public static void AddToCount(int value)
    {
        rwLock.EnterWriteLock();
        try
        {
            count += value;
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }
}

//Задача 2
//Есть "сервер" в виде статического класса.

//У него есть переменная count (тип int) и два метода, которые позволяют эту переменную читать и писать: GetCount() и AddToCount(int value).

//К классу–"серверу" параллельно обращаются множество клиентов, которые в основном читают, но некоторые добавляют значение к count.

//Нужно реализовать статический класс с методами  GetCount / AddToCount так, чтобы:
//•	читатели могли читать параллельно, не блокируя друг друга;
//•	писатели писали только последовательно и никогда одновременно;
//•	пока писатели добавляют и пишут, читатели должны ждать окончания записи.
