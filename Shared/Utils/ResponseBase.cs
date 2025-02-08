using System.Collections;
using System.Net;

namespace SisandBackend.Shared.Utils
{
    public class ResponseBase<T>
    {
        #region Consts

        private const string EMPTY_LIST = "No objects found";
        private const string DELETE_ROUTE = "Delete";
        private const int EMPTY = 0;

        #endregion

        #region Properties

        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int TotalCount { get; set; }

        #endregion

        #region Constructor

        public ResponseBase(string message, T data, int totalCount = 1)
        {
            Code = HttpStatusCode.OK;
            Message = GetEmptyListDefaultMessage(data, message);
            Data = data is null ? data : (T)GetEmptyObject(data);
            TotalCount = GetEmptyListDefaultValue(data, totalCount);
        }

        #endregion

        #region Methods

        private static string GetEmptyListDefaultMessage(T data, string message) =>
        NullOrZero(data) ? EMPTY_LIST : message;

        private static int GetEmptyListDefaultValue(T data, int totalCount) =>
            data is null ? EMPTY : totalCount;

        private static object GetEmptyObject(T data) =>
            data.ToString().Contains(DELETE_ROUTE) || data is null ? null : data;

        private static bool NullOrZero(T data)
        {
            if (data is not ICollection dataCollection)
                return data is null;

            return data is null || dataCollection.Count == EMPTY;
        }

        #endregion
    }
}
