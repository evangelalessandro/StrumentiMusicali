using StrumentiMusicali.Library.Entity;

namespace StrumentiMusicali.Library.Core
{
    public static class LoginData
    {
        public static void SetUtente(Utente utente)
        {
            utenteLogin = utente;
        }
        public static Utente utenteLogin { get; private set; }
    }
}
