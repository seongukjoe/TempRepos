
        public wKM1024i _driver; 
        public bool StopSpitting(int driverIndex)
        {
            bool result = true;
            if (isConnected == true)
            {
                var selectedHead = _deviceManager.GetInkjetHeadControlInfoList().FirstOrDefault(x => x.InkjetDriverIndex == driverIndex);
                result = selectedHead._driver.stopSpitting();
            }
            return result;
        }
        public bool SelectSpittingNozzle(int driverIndex, int[] nozzleOnOff, int nozzleCount)
        {
            bool result = true;

            if (isConnected == true)
            {
                int newNozzleIndex = 0;
                var selectedHead = _deviceManager.GetInkjetHeadControlInfoList().FirstOrDefault(x => x.InkjetDriverIndex == driverIndex);
                for (int i = 0; i < nozzleCount; i++)
                {
                    newNozzleIndex = (isReverseNozzle) ? nozzleCount - i - 1 : i;
                    if (selectedHead._driver.selectSpittingNozzle(newNozzleIndex, nozzleOnOff[newNozzleIndex] == 1) == false)
                        return false;
                }


            }

            return result;
        }
        public bool EnableSpitting(int driverIndex)
        {
            bool result = false;

            if (isConnected == true)
            {
                var selectedHead = _deviceManager.GetInkjetHeadControlInfoList().FirstOrDefault(x => x.InkjetDriverIndex == driverIndex);
                return selectedHead._driver.enableSpitting();
                //if (RJETDLL.RJETenableSpitting(driverIndex, 1) == RJETDLL.RETURN_OK)
                //{
                //    result = true;
                //}
            }

            return result;
        }
        public bool WaveformDownloadToDriver(int driverIndex, string InkjetheadName, double voltage1, double voltage2, double voltage3, double voltage4, double voltage5, double voltage6, double time1, double time2, double time3)
        {
            bool result = false;

            if (isConnected == true)
            {
                PCKM1024iWaveformDownloader.GetInstance().WaveformDownloadToDriver(driverIndex, voltage1, voltage2, voltage3, voltage4, voltage5, voltage6, time1, time2, time3);
                //foreach (WaveformDownloader waveformDownloader in wavefromDownloaderContainer)
                //{
                //    if (InkjetheadName == waveformDownloader.GetInkjetHeadName())
                //    {
                //        result = waveformDownloader.WaveformDownloadToDriver(driverIndex, voltage1, voltage2, voltage3, voltage4, voltage5, voltage6, time1, time2, time3);
                //        break;
                //    }
                //}
            }

            return result;
        }
        public bool EnableExternalTrigger(int idx, bool val)
        {
            bool result = false;

            if (isConnected == true)
            {
                var selectedHead = _deviceManager.GetInkjetHeadControlInfoList().FirstOrDefault(x => x.InkjetDriverIndex == idx);
                return selectedHead._driver.enableExternalTrigger(val);
                //if (RJETDLL.RJETenableSpitting(driverIndex, 1) == RJETDLL.RETURN_OK)
                //{
                //    result = true;
                //}
            }


            return result;
        }
