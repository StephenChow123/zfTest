using System;
using System.Collections.Generic;
using System.Text;

namespace AutoWelding.awdatabase
{
    class AwDbFactory
    {
        public enum DBType
        {
            Xml,
            MySql,
            SQLServer,
        }

        static private DBType cgDBType;

        /**********************************************************************************************
         * discription: ��ʼ����Ĭ��ΪMysql
         * 
         * 
         ***********************************************************************************************/
        public AwDbFactory()
        {
            cgDBType = DBType.Xml;
        }

        /**********************************************************************************************
      * discription: ����DB type
      * 
      * 
      ***********************************************************************************************/
        public void SetCgDBType(DBType dbType)
        {
            cgDBType = dbType;
        }

        /**********************************************************************************************
        * discription: ����Database engine
        * 
        * 
        ***********************************************************************************************/
        static public AwDbInterface CreateDBEngine()
        {
            AwDbInterface dbEngine = null;

            switch (cgDBType)
            {
                case DBType.MySql:                   
                    break;
                case DBType.SQLServer:
                    break;
                case DBType.Xml:
                    dbEngine = new AwDbXml();
                    break;
            }

            return dbEngine;
        }
    }
}
