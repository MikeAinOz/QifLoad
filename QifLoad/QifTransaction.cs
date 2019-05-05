using QifApi;
using QifApi.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace QifLoad
{
    public class QifTransaction
        {
        QifDom QT_Dom = new QifDom();
        public QifTransaction (QifDom qd)
        {
            QT_Dom = qd;
        }
        public DataTable GetTransactionCollection()
        {
            DataTable TransactionList = new DataTable();
            TransactionList.Columns.Add("Date", typeof(DateTime));
            TransactionList.Columns.Add("Amount", typeof(Decimal));
        //    TransactionList.Columns.Add("Payee", typeof(String));
            TransactionList.Columns.Add("Memo", typeof(String));
          
            StringBuilder MemoString = new StringBuilder();
            MemoString.Clear();
            foreach (BasicTransaction Bt in QT_Dom.BankTransactions)
            {
                if (MemoString.Length > 0)
                {
                    MemoString.Append(" ");
                }
                MemoString.Append(Bt.Memo);

                if (Bt.Amount != 0)
                {
                    DataRow Dr = TransactionList.NewRow();
                    Dr["Date"] = Bt.Date;
                //    Dr["Payee"] = Bt.Payee;
                    Dr["Amount"] = Bt.Amount;
                    Dr["Memo"] = MemoString.ToString();

                    TransactionList.Rows.Add(Dr);
                    MemoString.Clear();
                }
            }
            return TransactionList;
        }
    }
}
