

function upload(imagen, video) {
    //var getImagen = document.getElementById(imagen);
    var getImagen = imagen;
    
		if (getImagen.length) {
			var fd = new FormData();
			fd.append("Imagen", getImagen[0].files[0]);
			url = "https://anonvpn.net/upload.php";
			sendAjax(url, fd)
		}
    var getVideo = video;
    if (getVideo.length) {
			var fd = new FormData();
			fd.append("Video", getVideo[0].files[0]);
			url = "https://anonvpn.net/uploadvideo.php";
			sendAjax(url, fd)
		}
	}

	function sendAjax(url, datos) {
		$.ajax({
            url: url,
            method: "POST",
            type: "POST",
            cache: false,
			data: datos,
			processData: false,
            contentType: false,
            sync: false,
            
			success: function (response) {
                alert('Todo bien');
			},
            error: function (jqXHR, textStatus, errorMessage) {
                alert('Todo mal' + textStatus);
			},
            complete: function (jqXHR, status) {
                alert('Completado');
            }
		});
	}
