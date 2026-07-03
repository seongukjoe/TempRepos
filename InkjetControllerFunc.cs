        public bool UnselectSpittingNozzleAll()
        {
            bool result = false;

            if (IsDriverMatchingOK == true)
            {
                int[] nozzleOnOff = new int[NozzleCount];

                for (int setNozzleIndex = 0; setNozzleIndex < NozzleCount; setNozzleIndex++)
                {
                   // nozzleOnOff[setNozzleIndex] = RJETDLL.NOZZLE_OFF;
                    nozzleOnOff[setNozzleIndex] = 0;
                }

                result = InkDriver.SelectSpittingNozzle(InkjetDriverIndex, nozzleOnOff, NozzleCount);
            }

            return result;
        }

        public bool SelectedSpittingNozzle(int nozzleIndex, bool onOff)
        {
            bool result = false;

            if (IsDriverMatchingOK == true)
            {
                int nozzleOnOff = RJETDLL.NOZZLE_OFF;

                if (onOff == true)
                {
                    nozzleOnOff = RJETDLL.NOZZLE_ON;
                }

                result = InkDriver.SelectSpittingNozzle(InkjetDriverIndex, nozzleIndex, nozzleOnOff, NozzleCount);
            }

            return result;
        }
        public bool WaveformDownloadForDropwatcher()
        {
            bool result = false;
            int i = 0;

            if (IsDriverMatchingOK == true)
            {
                result = InkjetHeadDriver.GetInstance().WaveformDownloadToDriver(InkjetDriverIndex, HeadType, WaveformVoltage1, WaveformVoltage2, WaveformVoltage3, WaveformVoltage4, WaveformVoltage5, WaveformVoltage6,
                                                            WaveformTime1, WaveformTime2, WaveformTime3, DropWatcherVolt);
                //if (result == false)
                //{                   
                //        result = InkjetHeadDriver.GetInstance().WaveformDownloadToDriver(InkjetDriverIndex, HeadType, WaveformVoltage1, WaveformVoltage2, WaveformVoltage3, WaveformVoltage4, WaveformVoltage5, WaveformVoltage6,
                //              WaveformTime1, WaveformTime2, WaveformTime3, DropWatcherVolt);                  
                //}
                //result = InkDriver.WaveformDownloadToDriver(InkjetDriverIndex, HeadType, WaveformVoltage1, WaveformVoltage2, WaveformVoltage3, WaveformVoltage4, WaveformVoltage5, WaveformVoltage6,
                //           
               // WaveformTime1, WaveformTime2, WaveformTime3, DropWatcherVolt);


            }

            return result;
        }
        private bool StartLedStrobe()
        {
            bool result = false;
            return InkjetHeadDriver.GetInstance().InkjetLEDControl.enableLED(1); 
            if (RJETDLL.LEDstart(0) == RJETDLL.RETURN_OK)
            {
                result = true;
            }

            return result;
        }
        public bool EnableSpitting()
        {
            bool result = false;
            
            if (IsDriverMatchingOK == true)
            {
                result = InkDriver.EnableSpitting(InkjetDriverIndex);

                
            }

            return result;
        }
        public bool EnableExternalTrigger(bool val)
        {
            return InkDriver.EnableExternalTrigger(InkjetDriverIndex, val);
        }
        public bool StopSpitting()
        {
            return InkDriver.StopSpitting(InkjetDriverIndex); 
        }
