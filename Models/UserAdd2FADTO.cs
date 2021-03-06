using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SITConnect.Models
{
    public class UserAdd2FADTO
    {
        [Required]
        [RegularExpression(@"^(\d{1,3}|\d{1,4})$", ErrorMessage = "Please give a valid country code")]
        public string country_code { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please give a valid phone number")]
        public string phone_no { get; set; }

        [Required]
        public string token { get; set; }

        public static List<SelectListItem> countries = new List<SelectListItem>()
                                       {
                                        new SelectListItem{Value="213", Text="Algeria (+213)"},
new SelectListItem{Value="376", Text="Andorra (+376)"},
new SelectListItem{Value="244", Text="Angola (+244)"},
new SelectListItem{Value="1264", Text="Anguilla (+1264)"},
new SelectListItem{Value="1268", Text="Antigua &amp; Barbuda (+1268)"},
new SelectListItem{Value="54", Text="Argentina (+54)"},
new SelectListItem{Value="374", Text="Armenia (+374)"},
new SelectListItem{Value="297", Text="Aruba (+297)"},
new SelectListItem{Value="61", Text="Australia (+61)"},
new SelectListItem{Value="43", Text="Austria (+43)"},
new SelectListItem{Value="994", Text="Azerbaijan (+994)"},
new SelectListItem{Value="1242", Text="Bahamas (+1242)"},
new SelectListItem{Value="973", Text="Bahrain (+973)"},
new SelectListItem{Value="880", Text="Bangladesh (+880)"},
new SelectListItem{Value="1246", Text="Barbados (+1246)"},
new SelectListItem{Value="375", Text="Belarus (+375)"},
new SelectListItem{Value="32", Text="Belgium (+32)"},
new SelectListItem{Value="501", Text="Belize (+501)"},
new SelectListItem{Value="229", Text="Benin (+229)"},
new SelectListItem{Value="1441", Text="Bermuda (+1441)"},
new SelectListItem{Value="975", Text="Bhutan (+975)"},
new SelectListItem{Value="591", Text="Bolivia (+591)"},
new SelectListItem{Value="387", Text="Bosnia Herzegovina (+387)"},
new SelectListItem{Value="267", Text="Botswana (+267)"},
new SelectListItem{Value="55", Text="Brazil (+55)"},
new SelectListItem{Value="673", Text="Brunei (+673)"},
new SelectListItem{Value="359", Text="Bulgaria (+359)"},
new SelectListItem{Value="226", Text="Burkina Faso (+226)"},
new SelectListItem{Value="257", Text="Burundi (+257)"},
new SelectListItem{Value="855", Text="Cambodia (+855)"},
new SelectListItem{Value="237", Text="Cameroon (+237)"},
new SelectListItem{Value="1", Text="Canada (+1)"},
new SelectListItem{Value="238", Text="Cape Verde Islands (+238)"},
new SelectListItem{Value="1345", Text="Cayman Islands (+1345)"},
new SelectListItem{Value="236", Text="Central African Republic (+236)"},
new SelectListItem{Value="56", Text="Chile (+56)"},
new SelectListItem{Value="86", Text="China (+86)"},
new SelectListItem{Value="57", Text="Colombia (+57)"},
new SelectListItem{Value="269", Text="Comoros (+269)"},
new SelectListItem{Value="242", Text="Congo (+242)"},
new SelectListItem{Value="682", Text="Cook Islands (+682)"},
new SelectListItem{Value="506", Text="Costa Rica (+506)"},
new SelectListItem{Value="385", Text="Croatia (+385)"},
new SelectListItem{Value="53", Text="Cuba (+53)"},
new SelectListItem{Value="90392", Text="Cyprus North (+90392)"},
new SelectListItem{Value="357", Text="Cyprus South (+357)"},
new SelectListItem{Value="42", Text="Czech Republic (+42)"},
new SelectListItem{Value="45", Text="Denmark (+45)"},
new SelectListItem{Value="253", Text="Djibouti (+253)"},
new SelectListItem{Value="1809", Text="Dominica (+1809)"},
new SelectListItem{Value="1809", Text="Dominican Republic (+1809)"},
new SelectListItem{Value="593", Text="Ecuador (+593)"},
new SelectListItem{Value="20", Text="Egypt (+20)"},
new SelectListItem{Value="503", Text="El Salvador (+503)"},
new SelectListItem{Value="240", Text="Equatorial Guinea (+240)"},
new SelectListItem{Value="291", Text="Eritrea (+291)"},
new SelectListItem{Value="372", Text="Estonia (+372)"},
new SelectListItem{Value="251", Text="Ethiopia (+251)"},
new SelectListItem{Value="500", Text="Falkland Islands (+500)"},
new SelectListItem{Value="298", Text="Faroe Islands (+298)"},
new SelectListItem{Value="679", Text="Fiji (+679)"},
new SelectListItem{Value="358", Text="Finland (+358)"},
new SelectListItem{Value="33", Text="France (+33)"},
new SelectListItem{Value="594", Text="French Guiana (+594)"},
new SelectListItem{Value="689", Text="French Polynesia (+689)"},
new SelectListItem{Value="241", Text="Gabon (+241)"},
new SelectListItem{Value="220", Text="Gambia (+220)"},
new SelectListItem{Value="7880", Text="Georgia (+7880)"},
new SelectListItem{Value="49", Text="Germany (+49)"},
new SelectListItem{Value="233", Text="Ghana (+233)"},
new SelectListItem{Value="350", Text="Gibraltar (+350)"},
new SelectListItem{Value="30", Text="Greece (+30)"},
new SelectListItem{Value="299", Text="Greenland (+299)"},
new SelectListItem{Value="1473", Text="Grenada (+1473)"},
new SelectListItem{Value="590", Text="Guadeloupe (+590)"},
new SelectListItem{Value="671", Text="Guam (+671)"},
new SelectListItem{Value="502", Text="Guatemala (+502)"},
new SelectListItem{Value="224", Text="Guinea (+224)"},
new SelectListItem{Value="245", Text="Guinea - Bissau (+245)"},
new SelectListItem{Value="592", Text="Guyana (+592)"},
new SelectListItem{Value="509", Text="Haiti (+509)"},
new SelectListItem{Value="504", Text="Honduras (+504)"},
new SelectListItem{Value="852", Text="Hong Kong (+852)"},
new SelectListItem{Value="36", Text="Hungary (+36)"},
new SelectListItem{Value="354", Text="Iceland (+354)"},
new SelectListItem{Value="91", Text="India (+91)"},
new SelectListItem{Value="62", Text="Indonesia (+62)"},
new SelectListItem{Value="98", Text="Iran (+98)"},
new SelectListItem{Value="964", Text="Iraq (+964)"},
new SelectListItem{Value="353", Text="Ireland (+353)"},
new SelectListItem{Value="972", Text="Israel (+972)"},
new SelectListItem{Value="39", Text="Italy (+39)"},
new SelectListItem{Value="1876", Text="Jamaica (+1876)"},
new SelectListItem{Value="81", Text="Japan (+81)"},
new SelectListItem{Value="962", Text="Jordan (+962)"},
new SelectListItem{Value="7", Text="Kazakhstan (+7)"},
new SelectListItem{Value="254", Text="Kenya (+254)"},
new SelectListItem{Value="686", Text="Kiribati (+686)"},
new SelectListItem{Value="850", Text="Korea North (+850)"},
new SelectListItem{Value="82", Text="Korea South (+82)"},
new SelectListItem{Value="965", Text="Kuwait (+965)"},
new SelectListItem{Value="996", Text="Kyrgyzstan (+996)"},
new SelectListItem{Value="856", Text="Laos (+856)"},
new SelectListItem{Value="371", Text="Latvia (+371)"},
new SelectListItem{Value="961", Text="Lebanon (+961)"},
new SelectListItem{Value="266", Text="Lesotho (+266)"},
new SelectListItem{Value="231", Text="Liberia (+231)"},
new SelectListItem{Value="218", Text="Libya (+218)"},
new SelectListItem{Value="417", Text="Liechtenstein (+417)"},
new SelectListItem{Value="370", Text="Lithuania (+370)"},
new SelectListItem{Value="352", Text="Luxembourg (+352)"},
new SelectListItem{Value="853", Text="Macao (+853)"},
new SelectListItem{Value="389", Text="Macedonia (+389)"},
new SelectListItem{Value="261", Text="Madagascar (+261)"},
new SelectListItem{Value="265", Text="Malawi (+265)"},
new SelectListItem{Value="60", Text="Malaysia (+60)"},
new SelectListItem{Value="960", Text="Maldives (+960)"},
new SelectListItem{Value="223", Text="Mali (+223)"},
new SelectListItem{Value="356", Text="Malta (+356)"},
new SelectListItem{Value="692", Text="Marshall Islands (+692)"},
new SelectListItem{Value="596", Text="Martinique (+596)"},
new SelectListItem{Value="222", Text="Mauritania (+222)"},
new SelectListItem{Value="269", Text="Mayotte (+269)"},
new SelectListItem{Value="52", Text="Mexico (+52)"},
new SelectListItem{Value="691", Text="Micronesia (+691)"},
new SelectListItem{Value="373", Text="Moldova (+373)"},
new SelectListItem{Value="377", Text="Monaco (+377)"},
new SelectListItem{Value="976", Text="Mongolia (+976)"},
new SelectListItem{Value="1664", Text="Montserrat (+1664)"},
new SelectListItem{Value="212", Text="Morocco (+212)"},
new SelectListItem{Value="258", Text="Mozambique (+258)"},
new SelectListItem{Value="95", Text="Myanmar (+95)"},
new SelectListItem{Value="264", Text="Namibia (+264)"},
new SelectListItem{Value="674", Text="Nauru (+674)"},
new SelectListItem{Value="977", Text="Nepal (+977)"},
new SelectListItem{Value="31", Text="Netherlands (+31)"},
new SelectListItem{Value="687", Text="New Caledonia (+687)"},
new SelectListItem{Value="64", Text="New Zealand (+64)"},
new SelectListItem{Value="505", Text="Nicaragua (+505)"},
new SelectListItem{Value="227", Text="Niger (+227)"},
new SelectListItem{Value="234", Text="Nigeria (+234)"},
new SelectListItem{Value="683", Text="Niue (+683)"},
new SelectListItem{Value="672", Text="Norfolk Islands (+672)"},
new SelectListItem{Value="670", Text="Northern Marianas (+670)"},
new SelectListItem{Value="47", Text="Norway (+47)"},
new SelectListItem{Value="968", Text="Oman (+968)"},
new SelectListItem{Value="680", Text="Palau (+680)"},
new SelectListItem{Value="507", Text="Panama (+507)"},
new SelectListItem{Value="675", Text="Papua New Guinea (+675)"},
new SelectListItem{Value="595", Text="Paraguay (+595)"},
new SelectListItem{Value="51", Text="Peru (+51)"},
new SelectListItem{Value="63", Text="Philippines (+63)"},
new SelectListItem{Value="48", Text="Poland (+48)"},
new SelectListItem{Value="351", Text="Portugal (+351)"},
new SelectListItem{Value="1787", Text="Puerto Rico (+1787)"},
new SelectListItem{Value="974", Text="Qatar (+974)"},
new SelectListItem{Value="262", Text="Reunion (+262)"},
new SelectListItem{Value="40", Text="Romania (+40)"},
new SelectListItem{Value="7", Text="Russia (+7)"},
new SelectListItem{Value="250", Text="Rwanda (+250)"},
new SelectListItem{Value="378", Text="San Marino (+378)"},
new SelectListItem{Value="239", Text="Sao Tome &amp; Principe (+239)"},
new SelectListItem{Value="966", Text="Saudi Arabia (+966)"},
new SelectListItem{Value="221", Text="Senegal (+221)"},
new SelectListItem{Value="381", Text="Serbia (+381)"},
new SelectListItem{Value="248", Text="Seychelles (+248)"},
new SelectListItem{Value="232", Text="Sierra Leone (+232)"},
new SelectListItem{Value="65", Text="Singapore (+65)"},
new SelectListItem{Value="421", Text="Slovak Republic (+421)"},
new SelectListItem{Value="386", Text="Slovenia (+386)"},
new SelectListItem{Value="677", Text="Solomon Islands (+677)"},
new SelectListItem{Value="252", Text="Somalia (+252)"},
new SelectListItem{Value="27", Text="South Africa (+27)"},
new SelectListItem{Value="34", Text="Spain (+34)"},
new SelectListItem{Value="94", Text="Sri Lanka (+94)"},
new SelectListItem{Value="290", Text="St. Helena (+290)"},
new SelectListItem{Value="1869", Text="St. Kitts (+1869)"},
new SelectListItem{Value="1758", Text="St. Lucia (+1758)"},
new SelectListItem{Value="249", Text="Sudan (+249)"},
new SelectListItem{Value="597", Text="Suriname (+597)"},
new SelectListItem{Value="268", Text="Swaziland (+268)"},
new SelectListItem{Value="46", Text="Sweden (+46)"},
new SelectListItem{Value="41", Text="Switzerland (+41)"},
new SelectListItem{Value="963", Text="Syria (+963)"},
new SelectListItem{Value="886", Text="Taiwan (+886)"},
new SelectListItem{Value="7", Text="Tajikstan (+7)"},
new SelectListItem{Value="66", Text="Thailand (+66)"},
new SelectListItem{Value="228", Text="Togo (+228)"},
new SelectListItem{Value="676", Text="Tonga (+676)"},
new SelectListItem{Value="1868", Text="Trinidad &amp; Tobago (+1868)"},
new SelectListItem{Value="216", Text="Tunisia (+216)"},
new SelectListItem{Value="90", Text="Turkey (+90)"},
new SelectListItem{Value="7", Text="Turkmenistan (+7)"},
new SelectListItem{Value="993", Text="Turkmenistan (+993)"},
new SelectListItem{Value="1649", Text="Turks &amp; Caicos Islands (+1649)"},
new SelectListItem{Value="688", Text="Tuvalu (+688)"},
new SelectListItem{Value="256", Text="Uganda (+256)"},
new SelectListItem{Value="380", Text="Ukraine (+380)"},
new SelectListItem{Value="971", Text="United Arab Emirates (+971)"},
new SelectListItem{Value="598", Text="Uruguay (+598)"},
new SelectListItem{Value="7", Text="Uzbekistan (+7)"},
new SelectListItem{Value="678", Text="Vanuatu (+678)"},
new SelectListItem{Value="379", Text="Vatican City (+379)"},
new SelectListItem{Value="58", Text="Venezuela (+58)"},
new SelectListItem{Value="84", Text="Vietnam (+84)"},
new SelectListItem{Value="84", Text="Virgin Islands - British (+1284)"},
new SelectListItem{Value="84", Text="Virgin Islands - US (+1340)"},
new SelectListItem{Value="681", Text="Wallis &amp; Futuna (+681)"},
new SelectListItem{Value="969", Text="Yemen (North)(+969)"},
new SelectListItem{Value="967", Text="Yemen (South)(+967)"},
new SelectListItem{Value="260", Text="Zambia (+260)"},
new SelectListItem{Value="263", Text="Zimbabwe (+263)"}

                            };

    }
}
