namespace MongoWebApiStarter;

internal static class Tasks
{
    /// <summary>
    /// Executes multiple async methods (mostly in parallel) and returns a Task containing a tuple of results
    /// </summary>
    /// <typeparam name="T1">The return type of the first async method to be executed</typeparam>
    /// <typeparam name="T2">The return type of the second async method to be executed</typeparam>
    /// <param name="first">The first async method to be executed</param>
    /// <param name="second">The second async method to be executed</param>
    public static async Task<(T1, T2)> Run<T1, T2>(Task<T1> first, Task<T2> second)
    {
        await Task.WhenAll(first, second);
        return (first.Result, second.Result);
    }

    /// <summary>
    /// Executes multiple async methods (mostly in parallel) and returns a Task containing a tuple of results
    /// </summary>
    /// <typeparam name="T1">The return type of the first async method to be executed</typeparam>
    /// <typeparam name="T2">The return type of the second async method to be executed</typeparam>
    /// <typeparam name="T3">The return type of the third async method to be executed</typeparam>
    /// <param name="first">The first async method to be executed</param>
    /// <param name="second">The second async method to be executed</param>
    /// <param name="third">The third async method to be executed</param>
    public static async Task<(T1, T2, T3)> Run<T1, T2, T3>(Task<T1> first, Task<T2> second, Task<T3> third)
    {
        await Task.WhenAll(first, second, third);
        return (first.Result, second.Result, third.Result);
    }

    /// <summary>
    /// Executes multiple async methods (mostly in parallel) and returns a Task containing a tuple of results
    /// </summary>
    /// <typeparam name="T1">The return type of the first async method to be executed</typeparam>
    /// <typeparam name="T2">The return type of the second async method to be executed</typeparam>
    /// <typeparam name="T3">The return type of the third async method to be executed</typeparam>
    /// <typeparam name="T4">The return type of the fourth async method to be executed</typeparam>
    /// <param name="first">The first async method to be executed</param>
    /// <param name="second">The second async method to be executed</param>
    /// <param name="third">The third async method to be executed</param>
    /// <param name="fourth">The fourth async method to be executed</param>
    public static async Task<(T1, T2, T3, T4)> Run<T1, T2, T3, T4>(Task<T1> first, Task<T2> second, Task<T3> third, Task<T4> fourth)
    {
        await Task.WhenAll(first, second, third, fourth);
        return (first.Result, second.Result, third.Result, fourth.Result);
    }

    /// <summary>
    /// Executes multiple async methods (mostly in parallel) and returns a Task containing a tuple of results
    /// </summary>
    /// <typeparam name="T1">The return type of the first async method to be executed</typeparam>
    /// <typeparam name="T2">The return type of the second async method to be executed</typeparam>
    /// <typeparam name="T3">The return type of the third async method to be executed</typeparam>
    /// <typeparam name="T4">The return type of the fourth async method to be executed</typeparam>
    /// <typeparam name="T5">The return type of the fifth async method to be executed</typeparam>
    /// <param name="first">The first async method to be executed</param>
    /// <param name="second">The second async method to be executed</param>
    /// <param name="third">The third async method to be executed</param>
    /// <param name="fourth">The fourth async method to be executed</param>
    /// <param name="fifth">The fifth async method to be executed</param>
    public static async Task<(T1, T2, T3, T4, T5)> Run<T1, T2, T3, T4, T5>(Task<T1> first, Task<T2> second, Task<T3> third, Task<T4> fourth, Task<T5> fifth)
    {
        await Task.WhenAll(first, second, third, fourth, fifth);
        return (first.Result, second.Result, third.Result, fourth.Result, fifth.Result);
    }
}