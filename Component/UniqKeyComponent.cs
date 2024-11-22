using System;

namespace CSOEngine.Component
{
    /// <summary>
   ///
    /// </summary>
    public abstract class UniqKeyComponent:CSODataComponent
    {
        private readonly int id_int = -1;
        private readonly string id_string = null;
        private readonly Type type;

        private bool int_id_valid;
        private bool string_id_valid;


        public int Id_int
        {
            get
            {
                if (!int_id_valid)
                {
                    throw new Exception("UniqKeyComponent: Id_int not set value");
                }
                return id_int;
            }
        }
        public string Id_string
        {
            get
            {
                if (!string_id_valid || string.IsNullOrEmpty(id_string))
                {
                    throw new Exception("UniqKeyComponent: Id_string not set value");
                }
                return id_string;
            }
        }

        public UniqKeyComponent(int id)
        {
            int_id_valid = true;
            id_int = id;
            type = GetType();
        }
        public UniqKeyComponent(string id)
        {
            string_id_valid = true;
            id_string = id;
            type = GetType();
        }

        public override int GetHashCode()
        {
            return GetHashCode(id_int, id_string,type);
        }

        public static int GetHashCode(int id_int,string id_string,Type type)
        {
            return HashCode.Combine(id_int, id_string,type);
        }

        public override string ToString()
        {
            return string.Format("int_id :{0} :string_id_ {1} : type: {2}", id_int, id_string,type);
        }
    }
}