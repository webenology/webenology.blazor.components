using System.Diagnostics;
using JetBrains.dotMemoryUnit;
using Xunit.Abstractions;

namespace webenology.blazor.components.shared.Tests;

public class Tests
{
    private readonly ITestOutputHelper _testOutputHelper;


    [Test] 
    [AssertTraffic(AllocatedSizeInBytes = 0)]
    public void Test1()
    {
        var str = "hello abi what are you up to, what does it look like from up there?";

        var results = str.Highlight("wh ar you");

        Assert.That(results, Is.EqualTo("hello abi <mark>wh</mark>at <mark>ar</mark>e <mark>you</mark> up to, " +
                        "<mark>wh</mark>at does it look like from up th<mark>er</mark>e?"));
    }

    [Test]
    [AssertTraffic(AllocatedSizeInBytes = 0)]
    public void Test2()
    {
        var str = "hello abi what are you up to, what does it look like from up there?";

        var results = str.Highlight("wh what ar you");

        Assert.That(results, Is.EqualTo("hello abi <mark>what</mark> <mark>ar</mark>e <mark>you</mark> up to, " +
                                        "<mark>what</mark> does it look like from up th<mark>er</mark>e?"));
    }

    [Test]
   
    public void it_should_do_a_large_amount_of_text()
    {
        var sw = new Stopwatch();
        sw.Start();
        var item =
            @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras sed tortor nulla. Mauris vitae elit aliquam lorem aliquam tempus. Mauris non augue feugiat, imperdiet leo ac, pretium lectus. Nulla leo dolor, vulputate nec posuere vitae, maximus sit amet diam. Vestibulum hendrerit, odio in feugiat ultricies, elit velit porta nulla, ut consequat nisi lectus at urna. Donec auctor venenatis mi, sit amet pharetra ligula tempor quis. Nullam lacus erat, gravida sit amet erat nec, accumsan sagittis massa. Nulla finibus erat vel elit ultricies lobortis. Vivamus lorem mauris, tempus a metus nec, commodo blandit nisl." +
            "Integer facilisis odio eget ipsum mollis pellentesque. Quisque ullamcorper consequat felis non venenatis. Pellentesque eget massa ornare, aliquet nulla id, lacinia felis. In neque nisl, ornare non pharetra in, porta sagittis justo. Suspendisse nec ante sit amet sem maximus varius. Cras commodo risus mollis bibendum maximus. Mauris ac lobortis lorem, lobortis laoreet lectus. Duis non vestibulum massa. Nulla vel blandit mi, rutrum rhoncus ante. Aliquam facilisis sagittis congue. Ut pharetra diam id hendrerit sodales. Phasellus malesuada consequat convallis." +
            "Integer mattis sodales leo, eget pretium purus tincidunt vitae. Curabitur nisl felis, placerat id fermentum a, fringilla non ipsum. Praesent a sagittis mauris. Donec interdum rhoncus dui, sit amet ultricies ligula gravida at. Duis non luctus ligula, sed vulputate justo. Sed ac placerat purus, vestibulum mattis orci. Maecenas ac erat tempus, dapibus odio a, consectetur risus. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Ut malesuada sed ante eu fringilla. Duis dictum nunc lectus, eget porttitor est gravida quis. Etiam suscipit urna vel sem tempor accumsan. Curabitur molestie urna posuere dolor dignissim congue. Mauris non mauris vel orci rhoncus auctor. Mauris sed sodales dolor, sed cursus erat. Etiam non vulputate eros, ac luctus erat. Sed molestie, tortor nec varius pretium, neque augue dignissim odio, id tempor metus urna interdum sem." +
            "Nulla lobortis elit a interdum elementum. Nullam a lacus ipsum. Integer maximus lectus at nulla tincidunt luctus. Sed maximus, dolor a elementum gravida, neque elit varius sem, a blandit justo sem vel tortor. Phasellus a massa ipsum. Donec a hendrerit augue. Etiam egestas nisl quis libero elementum dictum. Interdum et malesuada fames ac ante ipsum primis in faucibus. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus." +
            "Duis nec dolor at ex maximus porta non id nulla. Cras consequat luctus luctus. Nullam lacinia magna eget tempor blandit. Pellentesque id maximus lorem. Cras dictum malesuada enim, at tempor metus pretium non. Praesent dictum ipsum sed est fermentum finibus. Vivamus faucibus nulla convallis massa fermentum finibus. Mauris mauris lectus, vestibulum id condimentum sed, consequat non orci." +
            "Morbi et magna sit amet nulla ultrices finibus sed ultrices velit. Pellentesque quis egestas quam. Donec orci enim, ornare non elementum ut, varius non elit. Phasellus hendrerit malesuada odio, sed sollicitudin arcu ultrices in. Donec id ante quis urna mattis interdum at eget tortor. Morbi ac nibh nunc. Ut risus libero, venenatis ac lacus nec, posuere viverra diam. Mauris eget purus id lectus accumsan fringilla. Nulla eleifend ligula orci, sed venenatis nibh tincidunt a. Suspendisse lobortis ligula at tincidunt viverra. Pellentesque nec finibus tellus. In sit amet leo sed orci mollis varius vel a lacus. Donec enim orci, tempus sed cursus a, accumsan id turpis. Donec eu cursus lacus." +
            "Sed pharetra maximus odio, et accumsan ex consequat quis. Integer mattis auctor metus, eget ultrices risus porta non. Aenean diam eros, elementum nec euismod et, aliquet id libero. Proin ut ex mauris. Etiam finibus imperdiet metus, vitae vehicula felis vulputate ut. Sed vel ipsum dapibus, viverra enim quis, iaculis enim. Curabitur nulla arcu, gravida vitae facilisis consectetur, luctus vel felis. Curabitur viverra ipsum massa, vitae dapibus odio congue at. Aliquam et leo dui. Ut quis est magna." +
            "Nulla facilisi. Donec molestie interdum velit vel vestibulum. Sed sed lorem et urna rutrum cursus at a quam. Nullam rutrum sapien erat, vitae pharetra nisi consectetur et. Fusce imperdiet malesuada magna, et pretium risus feugiat vitae. Vestibulum interdum dolor sed lectus placerat, ut viverra dolor lacinia. Vivamus tincidunt ac arcu eu porttitor. Nam condimentum turpis in nisi molestie posuere. In placerat non dolor in pharetra." +
            "Integer commodo dictum laoreet. Nam quis mattis metus, a semper nulla. Aenean in lacus posuere, iaculis mauris a, scelerisque orci. Integer suscipit ipsum eget urna tempus molestie. Morbi nec iaculis odio, ac finibus est. Cras tincidunt nec purus a laoreet. Fusce id ex eget risus convallis aliquet non a ante. Mauris id justo nec dolor tincidunt elementum. Sed vulputate ipsum nulla, non iaculis magna imperdiet ut. Pellentesque hendrerit libero nec nibh accumsan, et tempor enim posuere. Morbi et magna ac nibh vulputate venenatis et sed lectus. Integer sit amet sapien diam." +
            "Donec sit amet nisl sed arcu ultrices ullamcorper. Etiam sollicitudin lorem vel ultricies luctus. Quisque tortor eros, gravida et nunc eu, faucibus convallis orci. Etiam lacinia purus et nibh finibus pellentesque. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nulla facilisi. Nulla at sodales nunc. Fusce ornare ipsum at lectus condimentum ultrices. Phasellus pulvinar eros nec velit porta, sed tempor erat commodo. Phasellus dapibus libero risus, auctor placerat magna eleifend et. Etiam odio felis, vestibulum nec dignissim ut, consectetur id augue. Sed luctus metus at laoreet posuere. Suspendisse risus mi, finibus sed vehicula ut, hendrerit feugiat elit. Nullam dictum commodo semper. Aliquam quis velit non leo lacinia feugiat ac in est. Ut vel lorem vitae felis condimentum hendrerit at facilisis erat." +
            "Ut consequat sem ut dictum faucibus. Fusce vitae tellus ipsum. Fusce finibus orci quam, eget porta sem condimentum ac. Quisque nec varius augue. Morbi sem nibh, bibendum pellentesque tincidunt in, volutpat sed orci. Donec laoreet quam augue, non posuere nisl hendrerit et. Ut vitae elit nec neque dictum varius. Nulla pretium sem nulla, at mollis ipsum maximus quis." +
            "Interdum et malesuada fames ac ante ipsum primis in faucibus. Cras fringilla vitae dolor ut ullamcorper. Proin lobortis euismod nisl, iaculis mattis leo. Ut tempus nisi vel sem vehicula ullamcorper. Nulla placerat nibh urna, eget sodales elit lacinia nec. Cras vitae rutrum mauris, ut lobortis neque. Fusce eget hendrerit libero. Etiam et quam tortor. Donec tempus ante dui, quis laoreet libero laoreet ac." +
            "Praesent at tristique leo. Nulla porta metus quam, a imperdiet enim suscipit ac. Aliquam sed erat sed leo blandit auctor. Etiam a varius elit, eget placerat dui. Aenean ac turpis sed libero porttitor eleifend. Pellentesque aliquam velit ac imperdiet pretium. Curabitur mattis augue sed viverra dapibus." +
            "Etiam in faucibus lectus. Praesent rutrum eget neque in blandit. Mauris ultricies vulputate dui, sed posuere augue ultricies sit amet. Donec eleifend ultricies orci id varius. Vivamus in eros augue. In sed risus sed libero fermentum molestie. Donec malesuada mi a magna luctus, nec venenatis lorem semper. Suspendisse lectus nulla, porta quis neque id, pretium consectetur sem. Nullam tincidunt sapien in massa interdum venenatis. Quisque malesuada egestas elementum. Suspendisse potenti." +
            "Duis a sem nec felis efficitur commodo eu quis erat. Suspendisse eget urna eget urna maximus hendrerit. Fusce nisl metus, venenatis et velit id, porta aliquam lacus. Sed eget dolor blandit, pharetra magna eu, tristique lorem. Curabitur fermentum sagittis neque in cursus. Donec augue nibh, posuere ut sodales sed, faucibus condimentum justo. Fusce ac fringilla dui, luctus egestas felis. Ut arcu sem, sagittis eu fringilla iaculis, venenatis vel tellus. Duis ornare tempus nisi et tincidunt." +
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec fringilla consequat nunc, eget tincidunt dolor fermentum ut. Aliquam quis neque nec odio consequat bibendum. Nam eros arcu, mattis nec justo non, facilisis semper augue. Aenean tempus vulputate faucibus. Nam ultricies pellentesque mi vel elementum. Nunc venenatis ex quis lorem mattis fringilla. Donec sed orci orci. Donec ullamcorper interdum massa. Pellentesque laoreet nisi ac nunc suscipit, ut laoreet augue semper. Integer viverra bibendum arcu at aliquam. Maecenas vitae leo id lectus volutpat posuere ut eu metus. Proin vulputate, erat volutpat accumsan convallis, nulla purus tristique sem, volutpat iaculis est nisi at leo. Ut vitae risus semper, tempor augue imperdiet, consequat tortor. Vestibulum ac sem in velit gravida rutrum." +
            "Nunc sodales aliquam ex vel tincidunt. Praesent vel auctor ex. Morbi eget dolor nec enim vehicula mollis. Nulla facilisi. Duis accumsan elementum mauris, vel posuere libero accumsan sit amet. Morbi sed felis mauris. Nullam et elit malesuada magna ullamcorper pellentesque vel vehicula nisi. Aenean cursus justo at lacus interdum interdum. Pellentesque purus mauris, finibus id lacus tristique, lacinia dictum turpis. Aliquam ut efficitur mauris, a lacinia lorem." +
            "Nam auctor posuere quam vitae pharetra. Quisque pharetra interdum augue quis gravida. Pellentesque facilisis augue ut nulla semper maximus. Cras venenatis nulla velit, id ornare nunc congue sed. Etiam risus nisi, laoreet nec neque vel, ultrices tristique felis. Mauris ac risus a turpis blandit iaculis. Nam ultricies nulla non commodo feugiat. In nec condimentum erat. Ut egestas lectus at eros aliquet ultrices. Suspendisse potenti." +
            "Sed finibus porttitor congue. Nulla ac vehicula purus. Cras vitae lobortis lacus. Ut posuere pretium vehicula. Cras volutpat cursus elit, ac malesuada augue porttitor ut. Phasellus malesuada tincidunt laoreet. Morbi sagittis elit at semper ornare. Vestibulum in viverra erat. Sed fringilla odio leo, ac pretium dui interdum facilisis. Duis cursus ornare dolor. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. In hac habitasse platea dictumst. Aenean ut interdum mauris. Aliquam in tincidunt nisi. Nam eu ligula urna." +
            "Sed urna nunc, condimentum a porta vitae, vulputate sit amet lorem. Aliquam quam leo, pellentesque nec placerat eget, faucibus a sapien. Sed luctus, diam eleifend dictum malesuada, lorem nulla bibendum mi, ac pellentesque quam nisi vitae quam. Cras mattis faucibus sapien, vel maximus eros vestibulum non. In eget orci ornare, vehicula nisl nec, convallis lacus. Donec fermentum tristique lectus, vitae ultrices nisl luctus pretium. Duis dolor nulla, varius sed dui vulputate, tempor aliquet est. Proin accumsan vulputate efficitur. Phasellus quis venenatis odio, eu placerat augue. Aliquam blandit aliquam justo, id viverra massa fermentum sed. Nulla facilisi.";

        var search = "velit sem augue eim faci vel just matt eg lu veh lla";
        var iterations = 0;
        for (var i = 0; i < search.Length; i++)
        {
            var b = search.Substring(0, i);
            item.Highlight(b);
            iterations++;
        }

        sw.Stop();

        Assert.True(true, $"Elapsed: {sw.Elapsed}");
        Assert.AreEqual(52, iterations);

        Console.WriteLine($"search took: {sw.Elapsed}");
    }
}