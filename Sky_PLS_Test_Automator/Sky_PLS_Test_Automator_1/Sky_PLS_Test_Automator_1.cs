/*
****************************************************************************
*  Copyright (c) 2023,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

dd/mm/2023	1.0.0.1		XXX, Skyline	Initial version
****************************************************************************
*/

namespace Sky_PLS_Test_Automator_1
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			string regressionTest = engine.GetScriptParam("Regression Test").Value;
			if (string.IsNullOrEmpty(regressionTest) || !regressionTest.StartsWith("REG"))
				engine.ExitFail("Invalid regression test ID provided, should start with 'REG'");

			var dms = engine.GetDms();
			IDmsElement plsTesterElement = dms.GetElement("PLS Tester");
			if (plsTesterElement == null)
				engine.ExitFail("No 'PLS Tester' element found");

			Element plsTesterElement2 = engine.FindElement("PLS Tester");
			if (plsTesterElement2 == null)
				engine.ExitFail("No 'PLS Tester' element found 2");

			IDmsTable testCaseTable = plsTesterElement.GetTable(1000);
			if (testCaseTable == null)
				engine.ExitFail("No test table found in 'PLS Tester' element");

			string[] allTestCases = testCaseTable.GetDisplayKeys();
			string[] regressionTestCases = allTestCases.Where(tc => tc.Contains(regressionTest)).ToArray();
			Array.Sort(regressionTestCases);

			foreach (string regressionTestCase in regressionTestCases)
			{
				plsTesterElement2.SetParameter(1010, regressionTestCase, 1);
				WaitUntilTestSucceeded(regressionTestCase, plsTesterElement2, engine);
			}
		}

		public static void WaitUntilTestSucceeded(string regressionTest, Element plsTesterElement2, IEngine engine)
		{
			int maxRetries = 45;
			int counter = 0;

			do
			{
				engine.Sleep(2000);
				var lastRunStatus = Convert.ToInt32(plsTesterElement2.GetParameter(1007, regressionTest));
				if( lastRunStatus == 0)
					break;
				counter++;
			}
			while (counter < maxRetries);

			if (counter >= maxRetries)
				engine.ExitFail(String.Format("{1} failed", regressionTest));
		}

	}
}