using System;

namespace NyelvtechBead.Console
{
    public class Token
    {
        public Guid Id { get; set; }

        public int PositionInsentence { get; set; }

        public string Form { get; set; }

        public string Lemma { get; set; }

        public string UPOS { get; set; }

        public string XPOS { get; set; }

        public string Feats { get; set; }

        public int Head { get; set; }

        public string Deps { get; set; }

        public string Misc { get; set; }

        public string Mwe { get; set; }
    }
}
