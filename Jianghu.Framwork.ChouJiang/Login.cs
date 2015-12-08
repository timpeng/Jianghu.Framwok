using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Jianghu.Framwork.Repository.Model;
using Jianghu.Framwork.Repository.Repository;

namespace Jianghu.Framwork.ChouJiang
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private readonly MemberInfoRepository _memberInfoRepository = new MemberInfoRepository();
        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUid.Text) || string.IsNullOrWhiteSpace(txtPwd.Text))
            {
                MessageBox.Show("请输入登录信息");
                return;
            }
           var model=new Messager<MemberInfo>();
            try
            {
                 model = _memberInfoRepository.Login(txtUid.Text, txtPwd.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接服务器失败,请联系管理员!");
                return;
            }
            if (model.IsSuccess)
            {
                this.Hide();
                new Main(model.Model).ShowDialog();
            }
            else
            {
                MessageBox.Show(model.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
