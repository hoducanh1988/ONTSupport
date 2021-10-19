using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerContinuityTest.Function {
    public class myGlobal {

        public static SettingInformation mySetting = new SettingInformation();
        public static TestingInformation myTesting = new TestingInformation();

        public static PowerController myController = new PowerController();
        public static Dut myDut = new Dut();

    }
}
