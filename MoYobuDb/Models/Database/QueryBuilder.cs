using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Castle.Core.Internal;

namespace MoYobuDb.Models.Database
{
    public enum QueryType
    {
        Select,
        Insert,
        Update,
        Delete
    }

    /// <summary>
    ///     TEST
    /// </summary>
    public enum SortingType
    {
        Asc,
        Desc
    }

    public class QueryBuilder
    {
        private readonly List<string> _orderByList = new List<string>();
        private readonly List<string> _groupByList = new List<string>();
        private string _conditions;
        private Dictionary<string, string> _insertValues = new Dictionary<string, string>();
        private string _offset;
        private QueryType _queryType;
        private string[] _selectColumns;
        private string _table;
        private Dictionary<string, string> _updateSet = new Dictionary<string, string>();
        private readonly List<Join> joins = new List<Join>();
        
        public QueryBuilder OrderBy(string group, SortingType type)
        {
            _orderByList.Add(group + " " + type);
            return this;
        }
        
        public QueryBuilder GroupBy(string group)
        {
            _groupByList.Add(group);
            return this;
        }

        public QueryBuilder LeftJoin(string targetTable, string condition)
        {
            joins.Add(Join.Left(targetTable, condition));
            return this;
        }

        public QueryBuilder RightJoin(string targetTable, string condition)
        {
            joins.Add(Join.Right(targetTable, condition));
            return this;
        }

        public QueryBuilder InnerJoin(string targetTable, string condition)
        {
            joins.Add(Join.Inner(targetTable, condition));
            return this;
        }

        // ====================== TEST - KONEC ======================

        /// <summary>
        ///     Nastavení WHERE podmínek pro dotaz
        /// </summary>
        /// <param name="conditions">WHERE podmínky</param>
        /// <returns>QueryBuilder s podmínky pro WHERE</returns>
        public QueryBuilder Where(string conditions)
        {
            this._conditions = conditions;
            return this;
        }

        // SELECT
        /// <summary>
        ///     Vybrání SELECT dotazu a nastavení SELECT Columns
        /// </summary>
        /// <param name="args">SELECT Columns</param>
        /// <returns>QueryBuilder s nastavenými Columns</returns>
        public QueryBuilder Select(params string[] args)
        {
            _queryType = QueryType.Select; //??
            this._selectColumns = args;
            return this;
        }

        /// <summary>
        ///     Nastavení tabulky pro SELECT dotaz
        /// </summary>
        /// <param name="table">Tabulka</param>
        /// <returns>QueryBuilder s nastavenou tabulkou</returns>
        public QueryBuilder From(string table)
        {
            this._table = table;
            return this;
        }

        /// <summary>
        ///     Vrátí vytvořený dotaz pro SELECT
        /// </summary>
        /// <returns>SELECT query</returns>
        private string GetSelectQuery()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendFormat("SELECT {0} FROM {1}", String.Join(',', _selectColumns), _table);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                //throw;
            }

            if (!this.joins.IsNullOrEmpty())
            {
                foreach (var item in joins)
                {
                    sb.AppendFormat(" {0} JOIN {1} ON {2}", item.type, item.table, item.condition);
                }
            }


            if (this._conditions != null)
            {
                sb.AppendFormat(" WHERE {0}", _conditions);
            }

            // ====================== TEST - START ======================

            if (_groupByList.Count() != 0)
            {
                sb.AppendFormat(" GROUP BY");
                foreach (var item in _groupByList)
                {
                    sb.AppendFormat(" {0}", item);
                    if (!item.Equals(_groupByList.Last()))
                    {
                        sb.Append(",");
                    }
                }
            }

            if (_orderByList.Count() != 0)
            {
                sb.AppendFormat(" ORDER BY");
                foreach (var item in _orderByList)
                {
                    sb.AppendFormat(" {0}", item);
                    if (!item.Equals(_orderByList.Last()))
                    {
                        sb.Append(",");
                    }
                }
            }

            // TODO: Předělat
            if (this._offset != null)
            {
                sb.AppendFormat(_offset);
            }
            // ====================== TEST - KONEC ======================
            
            Debug.WriteLine("==============================");
            Debug.WriteLine("vrácená query");
            Debug.WriteLine(sb.ToString());
            Debug.WriteLine("==============================");
            return sb.ToString();
        }

        public QueryBuilder Offset(int startPageMultiplyByTen, int count)
        {
            this._offset = " OFFSET " + startPageMultiplyByTen + " ROWS FETCH NEXT " + count + " ROWS ONLY";
            return this;
        }

        // Insert
        /// <summary>
        ///     Vybrání INSERT dotazu a nastavení tabulky pro INSERT
        /// </summary>
        /// <param name="table">Tabulka</param>
        /// <returns>AueryBuilder s nastavenou tabulkou pro INSERT</returns>
        public QueryBuilder Insert(string table)
        {
            _queryType = QueryType.Insert;
            this._table = table;
            return this;
        }

        /// <summary>
        ///     Nastavení VALUES hodnot pro INSERT
        /// </summary>
        /// <param name="insertValues">values hodnoty</param>
        /// <returns>QueryBuilder s nastavenými hodnoty pro INSERT</returns>
        public QueryBuilder Values(Dictionary<string, string> insertValues)
        {
            this._insertValues = insertValues;
            return this;
        }

        /// <summary>
        ///     Vratí vytvořený dotaz pro INSERT dotaz
        /// </summary>
        /// <returns>INSERT query</returns>
        private string GetInsertQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0} ({1}) VALUES ({2})", _table, String.Join(',', this._insertValues.Keys),
                String.Join(',', this._insertValues.Values.Select(value => "'" + value + "'")));
            Debug.WriteLine(sb.ToString());
            return sb.ToString();
        }

        // Update
        /// <summary>
        ///     Nastavení SET parametru a values pro UPDATE
        /// </summary>
        /// <param name="updateSet">SET hodnoty</param>
        /// <returns>Vrací QueryBuilder s nastavenými SET hodnotami</returns>
        public QueryBuilder Set(Dictionary<string, string> updateSet)
        {
            this._updateSet = updateSet;
            return this;
        }

        /// <summary>
        ///     Vybrání UPDATE dotazu a nastavení tabulky pro UPDATE
        /// </summary>
        /// <param name="table">Tabulka</param>
        /// <returns>Vrací QueryBuilder s nastavenou tabulkou pro UPDATE</returns>
        public QueryBuilder Update(string table)
        {
            _queryType = QueryType.Update;
            this._table = table;
            return this;
        }

        /// <summary>
        ///     Vratí vytvořený dotaz pro UPDATE dotaz
        /// </summary>
        /// <returns>UPDATE query</returns>
        public string GetUpdateQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE {0} SET ", _table);

            foreach (var item in _updateSet)
            {
                sb.AppendFormat("{0} = '{1}'", item.Key, item.Value);
                //if (!_updateSet[item.Key].Equals(_updateSet.Last().Value))
                if (!item.Key.Equals(_updateSet.Last().Key))
                {
                    sb.Append(", ");
                }
            }

            if (this._conditions != null)
            {
                sb.AppendFormat(" WHERE {0}", _conditions);
            }

            return sb.ToString();
        }

        // Delete
        /// <summary>
        ///     Vybrání DELETE dotazu
        /// </summary>
        /// <param name="Table">Tabulka</param>
        /// <returns>AueryBuilder s nastavenou tabulkou pro INSERT</returns>
        public QueryBuilder Delete()
        {
            _queryType = QueryType.Delete;
            return this;
        }

        /// <summary>
        ///     Vratí vytvořený dotaz pro DELETE dotaz
        /// </summary>
        /// <returns>DELETE query</returns>
        private string GetDeleteQuery()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("DELETE FROM {0}", _table);
            if (this._conditions != null)
            {
                sb.AppendFormat(" WHERE {0}", _conditions);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Vrací konkrétni query podle dotazu uživatele
        /// </summary>
        /// <returns>Konkrétní query</returns>
        public string GetQuery()
        {
            string query = "";

            switch (_queryType)
            {
                case QueryType.Select:
                    query = this.GetSelectQuery();
                    break;
                case QueryType.Insert:
                    query = this.GetInsertQuery();
                    break;
                case QueryType.Update:
                    query = this.GetUpdateQuery();
                    break;
                case QueryType.Delete:
                    query = this.GetDeleteQuery();
                    break;
                default:
                    query = this.GetSelectQuery();
                    break;
            }

            Debug.WriteLine("=========\n\nquery: {0}", query);
            return query;
        }
    }
}