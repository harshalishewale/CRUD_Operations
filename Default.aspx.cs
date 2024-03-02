using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace CRUD_Operations
{
    public partial class Default : System.Web.UI.Page
    {
        string conn = @"Data Source=.;Initial Catalog=PhoneBook_DB;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                populateGridView();
            }
        }

        void populateGridView()
        {
            DataTable dtbl = new DataTable();
            using(SqlConnection sqlCon = new SqlConnection(conn))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("Select * FROM PhoneBook", sqlCon);
                sqlDa.Fill(dtbl);
            }
           if(dtbl.Rows.Count > 0)
            {
                gvPhoneBook.DataSource = dtbl;
                gvPhoneBook.DataBind();
            }
            else
            {
                dtbl.Rows.Add(dtbl.NewRow());
                gvPhoneBook.DataSource = dtbl;
                gvPhoneBook.DataBind();
                gvPhoneBook.Rows[0].Cells.Clear();
                gvPhoneBook.Rows[0].Cells.Add(new TableCell());
                gvPhoneBook.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvPhoneBook.Rows[0].Cells[0].Text = "No Data Found..!";
                gvPhoneBook.Rows[0].Cells[0].HorizontalAlign=HorizontalAlign.Center;
            }
        }

        protected void gvPhoneBook_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    using (SqlConnection sqlCon = new SqlConnection(conn))
                    {
                        sqlCon.Open();
                        string query = "Insert Into PhoneBook (FirstName, LastName, Contact, Email) values (@FirstName, @LastName, @Contact, @Email)";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@FirstName", (gvPhoneBook.FooterRow.FindControl("txtFirstNameFooter") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@LastName", (gvPhoneBook.FooterRow.FindControl("txtLastNameFooter") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Contact", (gvPhoneBook.FooterRow.FindControl("txtContactFooter") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Email", (gvPhoneBook.FooterRow.FindControl("txtEmailFooter") as TextBox).Text.Trim());
                        sqlCmd.ExecuteNonQuery();
                        populateGridView();
                        lblSuccessMsg.Text = "New Record Added...";
                        lblErrormsg.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {

                lblSuccessMsg.Text = ".";
                lblErrormsg.Text = ex.Message;
            }
        }

        protected void gvPhoneBook_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvPhoneBook.EditIndex = e.NewEditIndex;
            populateGridView();
        }

        protected void gvPhoneBook_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPhoneBook.EditIndex = -1;
            populateGridView();
        }


        protected void gvPhoneBook_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {

                using (SqlConnection sqlCon = new SqlConnection(conn))
                {
                    sqlCon.Open();
                    string query = "Update PhoneBook Set FirstName=@FirstName, LastName=@LastName, Contact=@Contact, Email=@Email where PhoneBookId=@iD";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@FirstName", (gvPhoneBook.Rows[e.RowIndex].FindControl("txtFirstName") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@LastName", (gvPhoneBook.Rows[e.RowIndex].FindControl("txtLastName") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Contact", (gvPhoneBook.Rows[e.RowIndex].FindControl("txtContact") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Email", (gvPhoneBook.Rows[e.RowIndex].FindControl("txtEmail") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvPhoneBook.DataKeys[e.RowIndex].Value.ToString()));

                    sqlCmd.ExecuteNonQuery();
                    gvPhoneBook.EditIndex = -1;
                    populateGridView();
                    lblSuccessMsg.Text = "Selected Record Updated...";
                    lblErrormsg.Text = "";
                }
            }
            catch (Exception ex)
            {

                lblSuccessMsg.Text = ".";
                lblErrormsg.Text = ex.Message;
            }
        }

        protected void gvPhoneBook_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

                using (SqlConnection sqlCon = new SqlConnection(conn))
                {
                    sqlCon.Open();
                    string query = "Delete from PhoneBook where PhoneBookId=@iD";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                   sqlCmd.Parameters.AddWithValue("@id", Convert.ToInt32(gvPhoneBook.DataKeys[e.RowIndex].Value.ToString()));

                    sqlCmd.ExecuteNonQuery();
                    populateGridView();
                    lblSuccessMsg.Text = "Selected Record Deleted...";
                    lblErrormsg.Text = "";
                }
            }
            catch (Exception ex)
            {

                lblSuccessMsg.Text = ".";
                lblErrormsg.Text = ex.Message;
            }
        }
    }

}