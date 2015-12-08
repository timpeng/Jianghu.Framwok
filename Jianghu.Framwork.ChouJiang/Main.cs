using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Jianghu.Framwork.Repository.Model;
using Jianghu.Framwork.Repository.Repository;

namespace Jianghu.Framwork.ChouJiang
{
    public partial class Main : Form
    {
        private readonly AvatarInfoRepository _avatarInfoRepository = new AvatarInfoRepository();
        public MemberInfo Member { get; set; }
        public Main(MemberInfo member)
        {
            this.Member = member;
            InitializeComponent();
            this.Closed += Main_Closed;
            var bs = new BindingSource();
            var data = _avatarInfoRepository.GetNameByUid(member.uID).ToList();
            var dic = new Dictionary<int, string>();
            dic.Add(0, "请选择");
            for (int i = 1; i <= data.Count(); i++)
            {
                dic.Add(i, data[i - 1]);
            }
            bs.DataSource = dic;
            cbmRole.DataSource = bs;
            cbmRole.ValueMember = "Key";
            cbmRole.DisplayMember = "Value";
            this.cbmRole.SelectedIndexChanged += cbmRole_SelectedIndexChanged;
            CheckForIllegalCrossThreadCalls = false;
        }

        void cbmRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cbmRole.SelectedValue) != 0)
            {
                this.lbGongxian.Text = _avatarInfoRepository.GetNameByUName(cbmRole.Text).aKillOtherTribe.ToString();
            }
            else
            {
                this.lbGongxian.Text = string.Empty;
            }
        }

        void Main_Closed(object sender, EventArgs e)
        {
            if (th != null)
            {
                if (th.ThreadState == ThreadState.Running)
                {
                    th.Abort();
                }
            }
            Application.Exit();
        }
        private Dictionary<long, string> prize;
        private Thread th;
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;
            if (string.IsNullOrEmpty(lbGongxian.Text))
            {
                this.btnStart.Enabled = true;
                MessageBox.Show("请选择用户角色！");
                return;
            }
            if (Convert.ToInt32(lbGongxian.Text) < 800)
            {
                this.btnStart.Enabled = true;
                MessageBox.Show("用户贡献不足！");
                return;
            }
            var elements = new List<KeyValuePair<long, double>>();
            prize = new Dictionary<long, string>();
            var result = this.groupBox.Controls;
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] is Label)
                {
                    result[i].ForeColor = Color.CadetBlue;
                    var label = result[i] as Label;
                    //label.AccessibleName//物品代码
                    //label.AccessibleDescription//物品属性代码
                    elements.Add(new KeyValuePair<long, double>(i, Convert.ToDouble(label.Tag)));
                    prize.Add(i, label.Text);
                }
            }
            var r = _avatarInfoRepository.CheckGongXian(Convert.ToInt16(lbGongxian.Text), cbmRole.Text);
            lock (r)
            {
                if (r.IsSuccess)
                {
                    th = new Thread(GetThread);
                    th.IsBackground = true;
                    th.Start(elements);
                }
                else
                {
                    this.btnStart.Enabled = true;
                    MessageBox.Show(r.Message);
                }
            }
        }
        void GetThread(object c)
        {
            var elements = c as List<KeyValuePair<long, double>>;
            //求出概率基数

            long basicNumber = 0;

            var array = new double[elements.Count];

            int m = 0;

            foreach (KeyValuePair<long, double> item in elements)
            {
                array[m] = item.Value;

                m++;
            }

            basicNumber = ToolMethods.GetBaseNumber(array);


            //判断设置的概率
            double allRate = 0;

            foreach (var item in elements)
            {
                allRate += item.Value;
            }

            if (allRate != 1)
            {
                MessageBox.Show("奖品概率设置错误！");

                Console.WriteLine(allRate);

                Console.ReadLine();

                return;
            }
            //抽奖
            Random random = new Random();

            long selectedElement = 0;
            long diceRoll = ToolMethods.GetRandomNumber(random, 1, basicNumber);

            var result = groupBox.Controls;
            long cumulative = 0;
            Label label = new Label();
            for (int i = 0; i < elements.Count; i++)
            {
                cumulative += (long)(elements[i].Value * basicNumber);
                if (result[i] is Label)
                {
                    label = result[i + 1] as Label;
                    if (label != null)
                    {
                        label.ForeColor = Color.Red;

                        if (diceRoll <= cumulative)
                        {
                            selectedElement = elements[i].Key;
                            var re = _avatarInfoRepository.GetChouJiang(label.AccessibleDescription, label.AccessibleName,
                            Convert.ToInt32(lbGongxian.Text), cbmRole.Text);
                            lbGongxian.Text = re.Model.aKillOtherTribe.ToString();
                            this.btnStart.Enabled = true;
                            break;
                        }
                    }
                }
                Thread.Sleep(500);
                label.ForeColor = Color.CadetBlue;
            }
            MessageBox.Show("抽取物品：【" + prize[selectedElement] + "】成功!", "系统消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
