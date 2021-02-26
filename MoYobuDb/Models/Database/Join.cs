namespace MoYobuDb.Models.Database
{
    public enum JoinType
    {
        INNER,
        LEFT,
        RIGHT
    }

    public class Join
    {
        private Join(JoinType type, string table, string condition)
        {
            this.type = type;
            this.table = table;
            this.condition = condition;
        }

        public JoinType type { get; }
        public string table { get; }
        public string condition { get; }


        public static Join Left(string table, string condition)
        {
            return new Join(JoinType.LEFT, table, condition);
        }

        public static Join Right(string table, string condition)
        {
            return new Join(JoinType.RIGHT, table, condition);
        }

        public static Join Inner(string table, string condition)
        {
            return new Join(JoinType.INNER, table, condition);
        }
    }
}