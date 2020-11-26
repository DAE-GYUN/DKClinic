﻿using DKClinic.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DKClinic.EmployeeProgram
{
    public partial class EmployeeManageQuestionnareControl : BaseUC
    {
        public EmployeeManageQuestionnareControl()
        {
            InitializeComponent();
            ReloadGridView();
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            EmployeeSelectFunctionControl employeeSelectFunctionControl = new EmployeeSelectFunctionControl();
            OnbtnCancelClicked(employeeSelectFunctionControl);
        }

        private void btnOpenResponse_Click(object sender, EventArgs e)
        {
            Questionnare questionnare = bdsQuestionnare.Current as Questionnare;
            //EmployeeModifyQuestionnareForm employeeModifyQuestionnareForm = new EmployeeModifyQuestionnareForm(questionnare.QuestionnareID);
            // 이벤트 핸들러 연결
            // 필요한거 하자
            //employeeModifyQuestionnareForm.ShowDialog();
        }

        private void connectedeventhandler(object sender, EventArgs e)
        {
            ReloadGridView();
        }

        private void ReloadGridView()
        {
            bdsQuestionnare.DataSource = Dao.Questionnare.GetWithDepartmentAndCustomerName();
        }
    }
}
