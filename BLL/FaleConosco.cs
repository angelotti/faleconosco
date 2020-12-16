using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace BLL
{
    public class FaleConosco
    {
        private static string SQL;
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _situacao;
        public string situacao
        {
            get { return _situacao; }
            set { _situacao = value; }
        }
        private string _nome;
        public string nome
        {
            get { return _nome; }
            set { _nome = value; }
        }
        private string _email;
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        private string _mensagem;
        public string mensagem
        {
            get { return _mensagem; }
            set { _mensagem = value; }
        }
        private string _resposta;
        public string resposta
        {
            get { return _resposta; }
            set { _resposta = value; }
        }

        public DataSet Listar()
        {
            DAO.Connection connection = new DAO.Connection();
            connection.ClearParameter();
            SQL = @"select Id, Situacao, Nome, Email, Mensagem, Resposta from tbl_FaleConosco ORDER BY ID DESC";
            return connection.ExecuteDataSet(SQL);
        }
        public DataSet ListarPendentes()
        {
            DAO.Connection connection = new DAO.Connection();
            connection.ClearParameter();
            SQL = @"select Id, Situacao, Nome, Email, Mensagem, Resposta from tbl_FaleConosco where Situacao='PENDENTE' ORDER BY ID DESC";
            return connection.ExecuteDataSet(SQL);
        }
        public DataSet ListarRespondidas()
        {
            DAO.Connection connection = new DAO.Connection();
            connection.ClearParameter();
            SQL = @"select Id, Situacao, Nome, Email, Mensagem, Resposta from tbl_FaleConosco where Situacao='RESPONDIDA' ORDER BY ID DESC";
            return connection.ExecuteDataSet(SQL);
        }
        public void Atualizar()
        {
            DAO.Connection connection = new DAO.Connection();
            connection.ClearParameter();
            SQL = @"update tbl_FaleConosco set Situacao='RESPONDIDA', Resposta = @resposta WHERE id = @id";
            connection.AddParameter("@resposta", SqlDbType.VarChar, _resposta);
            connection.AddParameter("@id", SqlDbType.Int, _id);
            connection.ExecuteDML(SQL);
        }
    }
}
