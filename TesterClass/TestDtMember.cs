using LookDAL.Mem.DAL;
using LookDB.Model.Member;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TesterClass
{
    public class TestDtMember:DALControllerBasic
    {
        DALDtMember test = new DALDtMember();
        public TestDtMember()
        {
                
        }
        public async Task TestSave()
        {
            DtMember dtMember = new DtMember();

            dtMember.MemberNoReg = "1212";
            dtMember.FrontName = "asda";
            dtMember.HP = "0909-";
            dtMember.IDCardNo = "adasd";
            dtMember.Marital = false;
            dtMember.Sex = "M";
            dtMember.DateBirth = "asda";
            dtMember.PlaceBirth = "asdsa'";
            dtMember.Email = "adadad#.vom";
            var hasil = await test.SimpanAsync<DtMember>(dtMember);

        }
    }
}
