namespace SistemaLogin.Helpers
{
    public class ConstMessages
    {
        // Mensagem de erro para log
        public static string ErroCriarUsuario = "Erro ao realizar criação de usuário";
        public static string ErroAtivarUsuario = "Erro ao realizar confirmação de conta por email de usuario";
        public static string ErroEnviarEmail = "Erro ao enviar email";
        public static string ErroFazerLogin = "Erro ao fazer login";
        public static string ErroRecuperarUsuarioEmail = "Erro ao recuperar usuário por email";
        public static string ErroGerarTokenRedefinirSenha = "Erro ao gerar token de redefinição de senha";
        public static string ErroAoRedefinirSenha = "Erro ao redefinir senha";

        // Status Http Sucesso
        public static int StatusOK200 = 200;
        public static int StatusCreated201 = 201;
        public static int StatusAccepted202 = 202;
        public static int StatusNoContent204 = 204;

        // Status Http Erro
        public static int StatusBadRequest400 = 400;
        public static int StatusUnauthorized401 = 401;
        public static int StatusForbiddenNoAccess403 = 403;
        public static int StatusNotFound404 = 404;
        public static int StatusConflict409 = 409;
        public static int StatusInternalServerError500 = 500;
    }
}
