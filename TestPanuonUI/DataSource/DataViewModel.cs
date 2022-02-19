using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BsbPcbaTestSpace
{
    public class DataViewModel:BindableObject
    {
        ObservableCollection<TestDataModel> _testDataModel = new ObservableCollection<TestDataModel>();
        private string _resultViewModel;
        private bool _isSelectTestAll = true;
        private bool _isSelectTestOne;
        private bool _isSelectAutoTest;
        private bool _isStopWhenFail;
        private bool _isStopWhenFinsh = true;
        private double _progressPrecent = 0;
        private bool _isAboutTestFlag;
        private string _strWindowTitle;

        public ObservableCollection<TestDataModel> TestDataModelView
        {
            get { return _testDataModel; }
            set
            {
                _testDataModel = value;
                SetValue(_testDataModel);
            }
        }
        public string ResultViewModel
        {
            get { return _resultViewModel; }
            set
            {
                _resultViewModel = value;
                SetValue(_resultViewModel);
            }
        }
        public bool isSelectTestAll
        {
            get { return _isSelectTestAll; }
            set
            {
                _isSelectTestAll = value;
                SetValue(_isSelectTestAll);
            }
        }
        public bool isSelectTestOne
        {
            get { return _isSelectTestOne; }
            set
            {
                _isSelectTestOne = value;
                SetValue(_isSelectTestOne);
            }
        }
        public bool isSelectAutoTest
        {
            get { return _isSelectAutoTest; }
            set
            {
                _isSelectAutoTest = value;
                SetValue(_isSelectAutoTest);
            }
        }
        public bool isStopWhenFail
        {
            get { return _isStopWhenFail; }
            set
            {
                _isStopWhenFail = value;
                SetValue(_isStopWhenFail);
            }
        }
        public bool isStopWhenFinsh
        {
            get { return _isStopWhenFinsh; }
            set
            {
                _isStopWhenFinsh = value;
                SetValue(_isStopWhenFinsh);
            }
        }
        public double progressPrecent
        {
            get { return _progressPrecent; }
            set
            {
                _progressPrecent = value;
                SetValue(_progressPrecent);
            }
        }
        public bool isAboutTestFlag
        {
            get { return _isAboutTestFlag; }
            set
            {
                _isAboutTestFlag = value;
                SetValue(_isAboutTestFlag);
            }

        }
        public string StrWindowTitle
        {
            get { return _strWindowTitle; }
            set
            {
                _strWindowTitle = value;
                SetValue(_strWindowTitle);
            }
        }
    }
}
