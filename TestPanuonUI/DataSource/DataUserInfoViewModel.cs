using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace BsbPcbaTestSpace
{
    public class UserInfo : BindableObject
    {
        //用户名
        private string _user;
        public string User
        {
            get { return _user; }
            set
            {
                _user = value;
                SetValue(_user);
            }
        }
        //用户等级
        private PositionLevelEnum _positionLevel;
        public PositionLevelEnum PositionLevel
        {
            get { return _positionLevel; }
            set
            {
                _positionLevel = value;
                SetValue(_positionLevel);
            }
        }
        //在线状态
        private OnlineEnum _onlineState;
        public OnlineEnum OnlineState
        {
            get { return _onlineState; }
            set
            {
                _onlineState = value;
                SetValue(_onlineState);
            }
        }
        //密码
        private string _psw;
        public string Psw
        {
            get { return _psw; }
            set
            {
                _psw = value;
                SetValue(_psw);
            }
        }
        //用户等级
        private string _position;
        public string Position
        {
            get { return _position; }
            set
            {
                _position = value;
                SetValue(_position);
            }
        }
        //在线（离线）
        private string _online;
        public string Online
        {
            get { return _online; }
            set
            {
                _online = value;
                SetValue(_online);
            }
        }
        //归属父用户
        private string _own;
        public string Own
        {
            get { return _own; }
            set
            {
                _own = value;
                SetValue(_own);
            }
        }
        private BitmapImage _showFaceImage;
        /// <summary>
        /// 人脸图片
        /// </summary>
        public BitmapImage ShowFaceImage
        {
            get { return _showFaceImage; }
            set
            {
                _showFaceImage = value;
                SetValue(_showFaceImage);
            }
        }
    }
    public class DataUserInfoViewModel:BindableObject
    {
        //用户列表
        private ObservableCollection<DataUserInfoViewModel> _ownUserList = new ObservableCollection<DataUserInfoViewModel>();
        public ObservableCollection<DataUserInfoViewModel> OwnUserList
        {
            get { return _ownUserList; }
            set
            {
                _ownUserList = value;
                SetValue(_ownUserList);
            }
        }
        //用户信息
        private UserInfo _presentUserInfo;
        public UserInfo PresentUserInfo
        {

            get { return _presentUserInfo; }
            set
            {
                _presentUserInfo = value;
                SetValue(_presentUserInfo);
            }
        }
    }
}
