namespace SisandBackend.Shared.Consts
{
    public static class ApiRouteConstants
    {
        public const string API_BASE_ROUTE = "api/v1/sisand-test/";

        public const string USER_GET_LIST_OK = "Sucesso ao buscar usuarios";
        public const string USER_GET_OK = "Sucesso ao buscar usuario";
        public const string USER_DELETE_OK = "Sucesso ao deletar usuario";
        public const string USER_UPDATE_OK = "Sucesso ao atualizar usuario";
        public const string USER_INSERT_OK = "Sucesso ao inserir usuario";

        public const string USER_GET_LIST_ERROR = "Erro ao buscar usuarios";
        public const string USER_GET_ERROR = "Erro ao buscar usuario";
        public const string USER_DELETE_ERROR = "Erro ao deletar usuario";
        public const string USER_UPDATE_ERROR = "Erro ao atualizar usuario";
        public const string USER_INSERT_ERROR = "Sucesso ao inserir usuario";

        public const string LOGIN_SUCCESS = "Sucesso ao efetuar login";

        public const string LOGIN_ERROR = "Erro ao efetuar login";

        public static string REFRESH_TOKEN_ERROR = "Refresh token invalido";

    }
}
