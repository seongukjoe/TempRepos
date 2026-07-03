 public async Task<bool> StartHeadSpitting(bool is1NozzleSpitting, CancellationToken token)
 {
     Logger.Debug("Execute StartHeadSpitting()");

     DropWatcherResultInfoList = new ObservableCollection<DropWatcherResult>();

     bool funcResult = false;

     try
     {
         if (SelectedHeadModuleInfo != null && SelectedHeadRowInfo != null)
         {
             //double rowOffsetMax = SelectedHeadModuleInfo.GetRowOffsetMax() + 0.5;

             if (positionMovingManager.CheckCurrentPositionSameByName(SelectedHeadModuleInfo.DropWatcherPosName, GetRowPositionOffsetInfoList()) == false)
             {
                 dialogManager.OpenDialog(DialogType.Information, "Fail Jetting", "It's not a drop position.");
                 return false;
             }
             else
             {
                 //await MoveRowOffsetPos(processCancelTokenSrc.Token);

                 //deviceManager.InkjetDriver.StopSpitting();
                 InkjetHeadDriver.GetInstance().InkjetLEDControl.disableLED(1);

                 SelectedHeadModuleInfo.HeadController.EnableExternalTrigger(false);
                 
              //   deviceManager.InkjetDriver.StopSpitting();


                 CodeDelay.DelayByTimespan(500);

                 funcResult = SetLedController();

                 if (funcResult == false)
                 {
                     alarmManager.AddAlarmByName("INKJET_FUNC_FAIL", "SET_LED_STROBE", LogSubtype.DropWatcherPage);
                     return false;
                 }

                 InkjetHeadControlInfo InkjetHeadController = SelectedHeadModuleInfo.HeadController;
                 InkjetHeadController.UnselectSpittingNozzleAll();   // Temporary, To make sure all nozzles are unselcted before nozzle select process 
                 await Task.Delay(300); 
                 if (InkjetHeadController != null)
                 {
                     if (is1NozzleSpitting == true)
                     {
                         //funcResult = InkjetHeadController.SelectSpittingNozzleRange(1, 1);
                         if(SelectedHeadRowInfo.RowNo == 1) funcResult = InkjetHeadController.SelectedSingleNozzle(1);
                         else if (SelectedHeadRowInfo.RowNo == 2) funcResult = InkjetHeadController.SelectedSingleNozzle(3);
                         else if (SelectedHeadRowInfo.RowNo == 3) funcResult = InkjetHeadController.SelectedSingleNozzle(2);
                         else if (SelectedHeadRowInfo.RowNo == 4) funcResult = InkjetHeadController.SelectedSingleNozzle(4);

                         logManager.SaveLog(LogType.Information, LogSubtype.DropWatcherPage, "Select 1st nozzle");
                     }
                     else
                     {
                         InkjetHeadRow headSpittingRowDefine = GetHeadRowDefineByIndex(SelectedHeadRowInfo.RowNo);

                         funcResult = InkjetHeadController.SelectSpittingNozzleRow(headSpittingRowDefine);

                         logManager.SaveLog(LogType.Information, LogSubtype.DropWatcherPage, "Select row spitting (Row) : " + headSpittingRowDefine.ToString());
                     }

                     if (funcResult == false)
                     {
                         alarmManager.AddAlarmByName("INKJET_FUNC_FAIL", "NOZZLE_SELECT", LogSubtype.DropWatcherPage);
                         return false;
                     }
                     CodeDelay.DelayByTimespan(200);
                     funcResult = InkjetHeadController.WaveformDownloadForDropwatcher();
                     //if (funcResult == false)
                     //{
                     //    alarmManager.AddAlarmByName("INKJET_FUNC_FAIL", "WAVEFORM_DOWNLOAD", LogSubtype.DropWatcherPage);
                     //    return false;
                     //}

                     logManager.SaveLog(LogType.Information, LogSubtype.DropWatcherPage, "Download waveform to driver : " + SelectedHeadModuleInfo.Name);

                     funcResult = StartLedStrobe();

                     if (funcResult == false)
                     {
                         alarmManager.AddAlarmByName("INKJET_FUNC_FAIL", "START_LED_STROBE", LogSubtype.DropWatcherPage);
                         return false;
                     }

                     funcResult = InkjetHeadController.EnableSpitting();

                     //if (funcResult == false)
                     //{
                     //    alarmManager.AddAlarmByName("INKJET_FUNC_FAIL", "ENABLE_SPITTING", LogSubtype.DropWatcherPage);
                     //    return false;
                     //}

                     logManager.SaveLog(LogType.Information, LogSubtype.DropWatcherPage, "Enable head spitting ready : " + SelectedHeadModuleInfo.Name);

        
                     logManager.SaveLog(LogType.Information, LogSubtype.DropWatcherPage, "Start LED Strobe");
                     funcResult = InkjetHeadController.EnableExternalTrigger(true);

                     funcResult = InkjetHeadDriver.GetInstance().InkjetLEDControl._ledControl.enableInternalTrigger(1000 / ParaFrequency, ParaDropCount);
                     if (funcResult == false)
                     {
                         alarmManager.AddAlarmByName("INKJET_FUNC_FAIL", "Fail to Set Internal Trigger", LogSubtype.DropWatcherPage);
                     }
                     else
                     {
                         logManager.SaveLog(LogType.Information, LogSubtype.DropWatcherPage, "Set Internal Trigger. ");
                     }
                     funcResult = InkjetHeadDriver.GetInstance().InkjetLEDControl._ledControl.enableLED(1);
                     if (funcResult == false)
                     {
                         alarmManager.AddAlarmByName("INKJET_FUNC_FAIL", "Fail To Enable LED", LogSubtype.DropWatcherPage);
                     }
                     else
                     {
                         logManager.SaveLog(LogType.Information, LogSubtype.DropWatcherPage, "Enable LED : ");
                     }
                //     funcResult = InkjetHeadController.StartSpitting(ParaFrequency, ParaDropCount); --260605 -sdy
                //     funcResult = deviceManager.InkjetDriver.StartSpitting(paraFrequencyCheck, paraDropCountCheck);

                     if (funcResult == false)
                     {
                         alarmManager.AddAlarmByName("INKJET_FUNC_FAIL", "START_SPITTING", LogSubtype.DropWatcherPage);
                     }
                     else
                     {
                         logManager.SaveLog(LogType.Information, LogSubtype.DropWatcherPage, "Start Head spitting (Freq, Conunt) : " + paraFrequencyCheck.ToString() + ", " + paraDropCountCheck.ToString());
                     }

                     int waitTimeMS = 0;

                     if (paraDropCountCheck == 0)
                     {
                         //waitTimeMS = maxTestSpittingTime;
                         waitTimeMS = (int)(ParaMaxSpittingTime * 60 * 1000);//단위 : 분
                     }
                     else
                     {
                         waitTimeMS = (int)((1.0 / paraFrequencyCheck) * paraDropCountCheck);
                     }

                     await Task.Delay(waitTimeMS + 300, token);

                     logManager.SaveLog(LogType.Information, LogSubtype.DropWatcherPage, "End Head spitting");
                    // MessageBox.Show("DropWatcher : Head spitting End");
                     if (!token.IsCancellationRequested)
                     {
                         InkjetHeadDriver.GetInstance().InkjetLEDControl._ledControl.disableLED(1);

                         SelectedHeadModuleInfo.HeadController.EnableExternalTrigger(false);
                         SelectedHeadModuleInfo.HeadController.StopSpitting();

                     }
                     //   deviceManager.InkjetDriver.StopSpitting();


                 }
             }
         }
     }
     catch (Exception ex)
     {
         InkjetHeadDriver.GetInstance().InkjetLEDControl._ledControl.disableLED(1);
         SelectedHeadModuleInfo.HeadController.EnableExternalTrigger(false);
         //        deviceManager.InkjetDriver.StopSpitting();

         Logger.Debug(ex, "StartSpitting()");
     }

     return true;
 }
