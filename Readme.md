# xBrowser #

xBrowser is style of client application that reactively displays content based on media type of the HTTP response.  There are a number of built in media types.  Ideally others can be plugged in.  Users can interact with the content based on the built in capabilities of the media type and hypermedia affordances.

## Why ##

Web browsers today are highly coupled to HTML as a content format.  HTML is an excellent format for rendering readable text.  However we can do better for interactive applications without the need to dynamically download more code to achieve our goals.  This browser is intended as a testing ground for use of other media types, especially hypermedia ones that can drive the web.


## How ##
For the moment, this implementation is focused on the Microsoft stack.  For displaying interactive user interfaces we support the use of `application/xaml+xml` with the infoset defined by `http://schemas.microsoft.com/winfx/2006/xaml/presentation`.  There is absolutely no reason why other UI technologies could not also be supported.

Currently there is only a WinRT client, but a WPF client will follow.  With some luck there will be a Windows Phone client also.  The core abilities of the browser will be available as a PCL library, which hopefully one day will not be constrained by the Windows Platform license constraint.

This project also heavily depends on the hypermedia tools available here: https://github.com/tavis-software



