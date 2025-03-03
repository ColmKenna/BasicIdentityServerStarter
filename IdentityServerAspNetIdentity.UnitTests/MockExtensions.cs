using Moq;

namespace IdentityServerAspNetIdentity.UnitTests;


    public static class MockExtensions
    {
        public static void SetupReturns<T, TResult>(
            this Mock<T> mock,
            Func<TResult> methodSelector,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector()).Returns(result);
        }

        public static void SetupReturns<T, TParam, TResult>(
            this Mock<T> mock,
            Func<TParam, TResult> methodSelector,
            TParam param,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector(param)).Returns(result);
        }

        public static void SetupReturns<T, TParam1, TParam2, TResult>(
            this Mock<T> mock,
            Func<TParam1, TParam2, TResult> methodSelector,
            TParam1 param1,
            TParam2 param2,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector(param1, param2)).Returns(result);
        }

        public static void SetupReturns<T, TParam1, TParam2, TParam3, TResult>(
            this Mock<T> mock,
            Func<TParam1, TParam2, TParam3, TResult> methodSelector,
            TParam1 param1,
            TParam2 param2,
            TParam3 param3,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector(param1, param2, param3)).Returns(result);
        }

        public static void SetupReturns<T, TParam1, TParam2, TParam3, TParam4, TResult>(
            this Mock<T> mock,
            Func<TParam1, TParam2, TParam3, TParam4, TResult> methodSelector,
            TParam1 param1,
            TParam2 param2,
            TParam3 param3,
            TParam4 param4,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector(param1, param2, param3, param4)).Returns(result);
        }

        public static void SetupReturns<T, TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(
            this Mock<T> mock,
            Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> methodSelector,
            TParam1 param1,
            TParam2 param2,
            TParam3 param3,
            TParam4 param4,
            TParam5 param5,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector(param1, param2, param3, param4, param5)).Returns(result);
        }

        public static void SetupReturnsAsync<T, TResult>(
            this Mock<T> mock,
            Func<Task<TResult>> methodSelector,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector()).ReturnsAsync(result);
        }

        public static void SetupReturnsAsync<T, TParam, TResult>(
            this Mock<T> mock,
            Func<TParam, Task<TResult>> methodSelector,
            TParam param,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector(param)).ReturnsAsync(result);
        }

        public static void SetupReturnsAsync<T, TParam1, TParam2, TResult>(
            this Mock<T> mock,
            Func<TParam1, TParam2, Task<TResult>> methodSelector,
            TParam1 param1,
            TParam2 param2,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector(param1, param2)).ReturnsAsync(result);
        }

        public static void SetupReturnsAsync<T, TParam1, TParam2, TParam3, TResult>(
            this Mock<T> mock,
            Func<TParam1, TParam2, TParam3, Task<TResult>> methodSelector,
            TParam1 param1,
            TParam2 param2,
            TParam3 param3,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector(param1, param2, param3)).ReturnsAsync(result);
        }

        public static void SetupReturnsAsync<T, TParam1, TParam2, TParam3, TParam4, TResult>(
            this Mock<T> mock,
            Func<TParam1, TParam2, TParam3, TParam4, Task<TResult>> methodSelector,
            TParam1 param1,
            TParam2 param2,
            TParam3 param3,
            TParam4 param4,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector(param1, param2, param3, param4)).ReturnsAsync(result);
        }

        public static void SetupReturnsAsync<T, TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(
            this Mock<T> mock,
            Func<TParam1, TParam2, TParam3, TParam4, TParam5, Task<TResult>> methodSelector,
            TParam1 param1,
            TParam2 param2,
            TParam3 param3,
            TParam4 param4,
            TParam5 param5,
            TResult result) where T : class
        {
            mock.Setup(m => methodSelector(param1, param2, param3, param4, param5)).ReturnsAsync(result);
        }
    }
