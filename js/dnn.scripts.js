//dnn.scripts is meant to be loaded dynamically 

function dnn_scripts()
{
	this.pns = 'dnn';
	this.ns = 'scripts';
}

dnn_scripts.prototype.ScriptBatchRequest = function (aSrc, aText, callBack)
{
	this.ref = aSrc;
	this.inline = aText;
	this.requests = [];
	this.callBack = callBack;
	this.numComplete = 0;
	this.numToComplete = 0;
}

dnn_scripts.prototype.ScriptBatchRequest.prototype = 
{
	load: function()
	{
		var ary = [];
		for (var i=0; i<this.ref.length; i++)
		{
			if (dnn.dom.scriptStatus(this.ref[i]) == '')	//not already loaded				
				ary.push(this.ref[i]);  //since opera loads all scripts synchronously we need to get an accurate count before starting the first one so we use a temp array.
		}
		this.numToComplete = ary.length;
		for (var i=0; i<ary.length; i++)
			this.requests.push(dnn.dom.loadScript(ary[i], null, dnn.createDelegate(this, this.asyncLoaded)));
		
		if (this.numToComplete == 0)
			this.asyncComplete();
	},
	
	asyncLoaded: function(sr)
	{
		if (sr.status == 'complete')	//if loaded add it
			this.numComplete +=1;
		
		if (this.numComplete == this.numToComplete)	//if all loaded
			this.asyncComplete();
	},
	
	asyncComplete: function()
	{
		for (var i=0; i<this.inline.length; i++)	//load inline scripts (these are sync)
			dnn.dom.loadScript(null, this.inline[i]);
		
		if (typeof(this.callBack) != 'undefined')	//if callback defined invoke
			this.callBack(this);
		
		this.disposeRequests();
	},
	
	
	disposeRequests: function()
	{
		for (var i=0; i<this.requests.length; i++)
			this.requests[i].dispose();	
	}
	
}

dnn.scripts = new dnn_scripts();
