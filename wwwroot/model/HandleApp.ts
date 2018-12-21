import Android from './Android';
import Ios from './Ios';

declare global {
  interface Window {
    opera: any,
    MSStream: any,
    isAndroid: any, // variable in app
    newTransaction: any[],
    AccountView: any,
    SettingApp: any,
  }
}


export default class HandleApp {
	
	static savePass(password: any) {

		var device = Android; //this.getMobileOperatingSystem();

		if (device) {
			let app = new device;
			app.savePass(password);

		}
	}


	static getMobileOperatingSystem() {
	  var userAgent = navigator.userAgent || navigator.vendor || window.opera;

	    if (/windows phone/i.test(userAgent)) {
	        return false; //Windowsphone;
	    }

	    if (/android/i.test(userAgent)) {
	        return Android;
	    }

	    if (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream) {
	        return Ios;
	    }

	    return false;
	}
}