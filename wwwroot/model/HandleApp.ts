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
    webkit: any
  }
}


export default class HandleApp {
	
	static savePass(password: any) {

		var getDevice = HandleApp.getMobileOperatingSystem(); //this.getMobileOperatingSystem();
		var device;

		if (getDevice == 'Ios') {
			device = Ios;
		} else {
			device = Android;
		}
		
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
	    else if (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream) {
	    	console.log("bbb");
	        return 'Ios';
	    }
	    else if (/chrome|android/i.test(userAgent)) {
	        return 'Android';
	    }
	    return false;
	}
}