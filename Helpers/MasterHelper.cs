namespace GymControlAPI.Helpers
{
    public class MasterHelper
    {
       private static MasterHelper _instance; //Instancia privada estática de la clase
       private static readonly object _lock = new object (); //Bloqueo para la creación de la instancia

       //Constructor privado para evitar instanciación externa
       private MasterHelper() { }
       //Propiedad pública estática para acceder a la instancia
       public static MasterHelper Instance
       {
            get
            {
                //Doble verificación para asegurar que solo se crea una instancia
                if (_instance == null) 
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new MasterHelper();
                        }
                    }
                }
                return _instance;
            }
       }
    }
}
