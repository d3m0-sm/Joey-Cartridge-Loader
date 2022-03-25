# Joey-Cartridge-Loader

Automatically checks and then loads the rom from the Joey Jr.

Requirements:
You have to associate the .GBC and .GBA extensions with your favourite emulator
Exp. mGBA

OS: Windows 10, Ubuntu/Kubuntu 20.04
<br /><br />
Configurations:
Configurations are stored in the "loader.conf".
You can change the following parameters:
<br />
<table>
  <tr>
    <td>
      letter=
    </td>
    <td>
      Used so tell the programm the drive letter of your Joey
    </td>
    <td>
      Examples: Win: (D, F, T); Lnx: (sdb, sdc, sdd) or auto
    </td>
  </tr>
  <tr>
    <td>
      ignore_checksum=
    </td>
    <td>
      If set to True, it doesen't check the checksome of your rom. 
    </td>
    <td>
      true/false
    </td>
  </tr>
  <tr>
    <td>
      ignore_rom_name=
    </td>
    <td>
      If set to True, it ignores if the file name is `ROM.GBx`
    </td>
    <td>
      true/false
    </td>
  </tr>
</table>
<br>
Note: ´letter=auto´ does only work on linux.


