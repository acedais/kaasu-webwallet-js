export default class Android {
	
	savePass(password: any) {
		if (window.isAndroid){
			window.isAndroid.savePass(password);
		}
	}
}