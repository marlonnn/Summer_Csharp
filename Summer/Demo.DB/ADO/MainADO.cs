using System;
using Summer.System.Data;
using Summer.System.Data.DbMapping;

namespace Demo.DB.ADO
{
    public class MainADO : SmrAdoTmplate<TMain>
    {

    }

    [TableAttribute("Main")]
    public class TMain
    {
        [FieldAttribute("Key", PrimaryKey = true)]
        public string Key;

        [FieldAttribute("Start")]
        public DateTime Start;

        [FieldAttribute("End")]
        public DateTime End;

        [FieldAttribute("Note")]
        public string Note;
    }

}
