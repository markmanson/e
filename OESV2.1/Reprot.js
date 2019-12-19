// JScript File

function  emptylstbox()
		{
		
		if (lstRight.item ) then
		{
		
		}
		
		
		
		}
		
		
		function ConDel()
		{
			var answer = confirm("Do you want to delete the candidate?");
			if(!answer)
			{
				window.event.returnValue = false;
			}
				
		}
<!--
selectedLst = ""
function MoveList(lstDist,orderby) {
  
  if((selectedLst.id == "lstLeft" && lstDist.id == "lstLeft") || (selectedLst.id=="lstorderby" && lstDist.id=="lstorderby") || (selectedLst.id=="lstRight" && lstDist.id=="lstRight") || (selectedLst==""))
  {
  
  }
  else
  {
  iLen = selectedLst.options.length;
  aValue = new Array(iLen);
  aText = new Array(iLen);
 
  iIndex = 0;
  var check=0;
  for (iCnt=0; iCnt < iLen; iCnt++) {
  elem = document.getElementById('lstorderby');
  check=0;
  for (var i=0;i< elem.length;i++)
  {
    if(elem.options[i].value==selectedLst.options[iCnt].value)
        check=1;
  }
      if (selectedLst.options[iCnt].selected && check!=1) {
      iOptLen = lstDist.options.length;
      lstDist.options.length = iOptLen+1;
      lstDist.options[iOptLen].value = selectedLst.options[iCnt].value;
      lstDist.options[iOptLen].text = selectedLst.options[iCnt].text;
      lstDist.options[iOptLen].selected = true;
      if(orderby)
      {
		aValue[iIndex] = selectedLst.options[iCnt].value;
		aText[iIndex] = selectedLst.options[iCnt].text;
		iIndex++;
      }
    } else {
      aValue[iIndex] = selectedLst.options[iCnt].value;
      aText[iIndex] = selectedLst.options[iCnt].text;
      iIndex++;
    }
  }
	selectedLst.options.length = iIndex;
	for (iCnt=0; iCnt < iIndex; iCnt++) {
		selectedLst.options[iCnt].value = aValue[iCnt];
		selectedLst.options[iCnt].text = aText[iCnt];
		selectedLst.options[iCnt].selected = false;
	}
	selObj=document.getElementById('lstorderby');
	for (var i = 0; i < selObj.length; i++)
selObj.options[i].selected = false;

selObj=document.getElementById('lstRight');
	for (var i = 0; i < selObj.length; i++)
selObj.options[i].selected = false;

selObj=document.getElementById('lstLeft');
	for (var i = 0; i < selObj.length; i++)
selObj.options[i].selected = false;
  //**************put data in hidden field**********
	Listedfields()
	orderybyfields()
  //alert(selectedLst.options[1].value)
  }
}

function RemoveFromList(me)
{
	iLen = me.options.length;
	aValue = new Array(iLen);
	aText = new Array(iLen);
	iIndex = 0;
	
	for (iCnt=0; iCnt < iLen; iCnt++) {
		if (me.options[iCnt].selected) {
		} else {
		aValue[iIndex] = me.options[iCnt].value;
		aText[iIndex] = me.options[iCnt].text;
		iIndex++;
		}
	}
	me.options.length = iIndex;
	for (iCnt=0; iCnt < iIndex; iCnt++) {
		me.options[iCnt].value = aValue[iCnt];
		me.options[iCnt].text = aText[iCnt];
		me.options[iCnt].selected = false;
	}
	orderybyfields()
}

function UnSelect(lstSrc,lstSrc1,me) {
  for (iCnt=0; iCnt < lstSrc.options.length; iCnt++) {
    lstSrc.options[iCnt].selected = false;
  }
   for (iCnt=0; iCnt < lstSrc1.options.length; iCnt++) {
    lstSrc1.options[iCnt].selected = false;
  }
  selectedLst=me
}

function SelectAll(lstSrc) {
  for (iCnt=0; iCnt < lstSrc.options.length; iCnt++) {
    lstSrc.options[iCnt].selected = true;
  }
}

function Listedfields()
{
	values=""
	for(iCnt=0;iCnt<document.fList.lstRight.options.length;iCnt++)
	{
		values = values + document.fList.lstRight.options[iCnt].value + ","
	}
	document.fList.hid_Right.value=values;
}

function orderybyfields()
{
	orderbyvalues=""
	for(iCnt=0;iCnt<document.fList.lstorderby.options.length;iCnt++)
	{
			orderbyvalues = orderbyvalues + document.fList.lstorderby.options[iCnt].value + ","
	}
	
	document.fList.hid_orderby.value=orderbyvalues;
}


