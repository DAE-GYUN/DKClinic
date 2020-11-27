﻿using DKClinic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DKClinic.EmployeeProgram
{
    public partial class EmployeeManageQuestionControl : BaseUC
    {
        public List<Question> BeforeQuestions { get; set; }
        public List<Question> AfterQuestions { get; set; }
        public Department Department { get; set; }

        public Employee ParentEmployee { get; set; }

        public EmployeeManageQuestionControl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode)
                return;

            ParentEmployee = ((MainForm)ParentForm).ConnectedEmployee;

            Department = Dao.Department.GetByPK(ParentEmployee.DepartmentID);

            BeforeQuestions = Dao.Question.GetByNewestVersionByDepartmentID(ParentEmployee.DepartmentID);

            AfterQuestions = new List<Question>(BeforeQuestions);

            ReloadGridView();
        }

        private void ReloadGridView()
        {
            bdsQuestion.DataSource = Dao.Question.SelectionNewestVersionByDepartmentID(
                Dao.Question.ConvertDepartmentAndTypeName(AfterQuestions), ParentEmployee.DepartmentID);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EmployeeModifyQuestionForm employeeModifyQuestionForm = new EmployeeModifyQuestionForm((int)Department.Count);
            employeeModifyQuestionForm.ShowDialog();

            // Insert
            // 맨 뒤에 추가할 경우
            // add + department count+1
            // 중간에 추가할 경우
            // add + Version+1 + department count+1
            // 바뀐 번호값에 대해 나머지 문제들 변화된 index + Version+1 add

            if(employeeModifyQuestionForm.IsChaneged)
            {
                // version붙이기
                employeeModifyQuestionForm.Question.Version =
                    Dao.Question.GetNewestVersionNumber(employeeModifyQuestionForm.Question.DepartmentID,
                    employeeModifyQuestionForm.Question.Index) + 1;
                // 추가
                AfterQuestions.Add(employeeModifyQuestionForm.Question);
                
                if(employeeModifyQuestionForm.Question.Index != Department.Count)
                {
                    for(int i = employeeModifyQuestionForm.Question.Index + 1;
                        i <= Department.Count;i++ )
                    {
                        Question question = 
                            AfterQuestions.FindAll(x => x.Index == i)
                                .OrderByDescending(x => x.Version)
                                .FirstOrDefault();
                        question.Version++;
                        AfterQuestions.Add(question);
                    }
                }

                Department.Count++;
            }

            ReloadGridView();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            EmployeeModifyQuestionForm employeeModifyQuestionForm = new EmployeeModifyQuestionForm((Question)bdsQuestion.Current, (int)Department.Count);
            employeeModifyQuestionForm.ShowDialog();

            // Update
            // index가 안변할경우
            // 변화된 값 + Version+1 add
            // index가 변할경우
            // 변화된 값 + Version+1 + 변화된 index add
            // 바뀐 번호값에 대해 나머지 문제들은 변화된 index + Version+1 add

            ReloadGridView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (WinformUtility.AskSure("정말 삭제하시겠습니까? (실제 반영은 일괄저장을 누를 때 반영됩니다)") == false)
                return;
            
            // delete
            // 삭제된 번호값에 대해 나머지 문제들 변화된 index + version+1 add
            // department count-1
            

            //AfterQuestions.Remove((Question)bdsQuestion.Current);
            
            ReloadGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 작업하는 사이에 데이터베이스에 변화가 있었나?
            List<Question> questions = new List<Question>();
            if (ParentEmployee.PositionID == 1)
                questions = Dao.Question.GetAll();
            else
                questions = Dao.Question.GetByDepartmentID(ParentEmployee.DepartmentID);
            
            // 있었다면 보내버린다(응?)
            if(questions.SequenceEqual(questions) == false)
            {
                MessageBox.Show("서버의 다른 변화를 감지하여 저장을 취소합니다. 처음부터 다시 시도해주세요.");
                btnGoBack.PerformClick();
            }

            // department 업데이트
            Dao.Department.Update(Department);

            // afterQuestion 업데이트(Insert)
            
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            EmployeeSelectFunctionControl employeeSelectFunctionControl = new EmployeeSelectFunctionControl();
            OnbtnCancelClicked(employeeSelectFunctionControl);
        }
    }
}
