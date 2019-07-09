
# OpenSong Web
OpenSongWeb aims to be a resource for worship teams to prepare for leading worship and present the set.  It uses the OpenSong format.
It also is a rewrite of a full-featured system created in spare time in 2012 found [here](http://wworship.24s2sanmarcos.org/).

### Building
	>     npm install -g webpack-cli
    >     npm install
    build or run from visual studio.
	

This will become a pre-packaged container at some point. 

### Bulk Data Imports
To see this thing work with actual data, or to add songs to the database in bulk, you can extract DataImport.zip (in the root of the project) to the folder specified in the app config file.  On boot the app will process the inbound folder and drop the results in the other two folders based on success or failure.

### Goals:
- Provide a repository of worship songs
-- Track song use, popularity, tagging, etc.
-- Allow users to create/upload songs
- Provide worship leader helps
-- CAPO and transpose songs, etc.
-- Build a worship set - order, capo, transpose, and annotate a set list.
-- Song sheet exports
-- Lead Sheet exports
- Provide worship set presentation
-- Projection scenarios
-- Shared, mobile scenarios - self-guided or presenter-guided
- Keep it Free (ads or donation model?)

## Road Map
There's lots of plans and ideas.  Will vet needs with worship leaders and adjust the below plan...

### Finished Features
- Viewing songs 
-- "all" 
-- by tags
-- individual song.
- Bulk import (OpenSong Format)
- song searching
-- server side
-- client filtering

### In-progress

### Planned Features 
------ to be ported ------
2. User authentication and mgmt
3. Set mgmt
4. presentation
5. capo
6. transpose
7. Song tags
8. set export
9. Conversion/Cooperation with ChordPro format?
10. bulk download?
------ new ideas ------
11. shared but guided presentation
12. Team Mgmt
13. CCLI reporting/tracking
14. Full service presentation (sermon/multimedia/bg/etc)

### Unicode and regional support
Some features of the original web app, particularly the RTF format export, do not support Unicode.  Other languages and locales should be intentionally supported, so that we can help the persecuted church for example.

### Formatting
Currently this application supports only the [OpenSong](http://www.opensong.org/) [format](https://sourceforge.net/projects/opensong/).  There is a plan being fleshed out to support chordii/chordpro format as well or even switch to it altogether.  If either format will ultimately become the native format remains to be seen, but we will start with OpenSong, to get the other features out there.
 
**OpenSong Format Pros:**
-- Customizeable, immediately-searchable XML song format
-- Set format can support other needs of a worship service, such as video, pictures, bible verses, slides, and so on.
-- Has a sensible song progression describing what portions of the song are planned in what sequence.
-- Good separation of concerns of definition of songs/sets vs visual/presentation of said sets.

**OpenSong Format Cons:**
-- Chords not being specified inline leads to positioning issues without mono-spaced fonts, and leads to positioning only being correct in one language.
-- Does not support tabs definitions AFAICT

**OpenSong Format Notes**
-- [File format spec](http://www.opensong.org/home/file-formats).

**ChordPro Pros:**
-- wide acceptance, and simplicity, of the format.
-- Chords are inline, not requiring mono-spaced fonts.
-- Built-in support for tablatures

**ChordPro Cons:**
-- Does not define a set format.

**ChordPro Notes**
Use [this regex](https://stackoverflow.com/questions/44314124/c-sharp-use-regex-to-remove-everything-inside-brackets-and-the-brackets-themselv) to build a searchable string. 
-- [chordii syntax explained](https://www.chordpro.org/chordpro/ChordPro-Directives.html)
-- [reference implementation source](https://github.com/ChordPro/chordpro)
 
## History
This app began as a ASP.NET MVC 3 web app written as a way to help worship leaders at a small church plant with planning and song sheets for small group and corporate worship.  Written to be completely compatible with the [OpenSong](http://www.opensong.org/) desktop application and [format](https://sourceforge.net/projects/opensong/)  - without requiring the OpenSong desktop application itself - the application provided users the ability to create or upload songs and sets into the database, share them, export them in RTF and HTML formats, share lyric sheets via web links, and even run a slideshow presentation of their sets.  

Seven years on, the web app had proven that the OpenSong format was viable despite no one in the churches AFAICT actually using the desktop app- they already had a feature-rich presentation software.  

I decided to rewrite the app on .NET Core so that it could be platform-neutral and open source it so that others could benefit and improve on the services it offers.

### Other resources for worship leaders
Here's a pretty decent [list](http://www.opensong.org/home/links).
