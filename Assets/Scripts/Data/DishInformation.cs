using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishInformation : MonoBehaviour
{
    public string DishInformationText(int cardID)
    {
        string text = null;

        switch (cardID)
        {
            case 0:
                text = "うすく切った牛肉とタマネギを甘辛く煮込んで、ご飯の上にのせた料理。明治時代に牛鍋をご飯にかけて食べたものがはじまり。";
                break;
            case 1:
                text = "チャーシューを多めにトッピングしたラーメン。チャーシューには、豚バラ肉や豚もも肉がよく使われる。";
                break;
            case 2:
                text = "肉やじゃがいもを、しょう油・砂糖・みりんで煮込んだ料理。余ったらカレーやコロッケにアレンジしやすい優れもの！";
                break;
            case 3:
                text = "かにを混ぜて焼いた玉子をご飯にのせてとろみのあるタレをかけた中華料理。実は日本生まれで中国に同じような料理はない。";
                break;
            case 4:
                text = "生卵を落としたかけうどん。卵の白身を雲、黄身を月に見立てた風流な料理。夜空に見立てた海苔をいれることもある。";
                break;
            case 5:
                text = "ゆでたじゃがいもをマヨネーズで和えたサラダ。ゆで卵、キュウリ、リンゴなど、いれるものに作る人の個性が出る。";
                break;
            case 6:
                text = "握った酢飯（シャリ）に魚の切り身など（ネタ）をのせた日本料理。回転ずしのコンベアはほぼ100％が石川県で作られているそう。";
                break;
            case 7:
                text = "魚の切り身に小麦粉などの粉をまぶしてバターで焼いたフランス料理。外側はカリッと、内側は柔らかい食感が絶品！";
                break;
            case 8:
                text = "アメリカ生まれの2枚貝のスープ。牛乳をベースとした白いクリームスープと、赤いトマトスープの2種類がある。";
                break;
            case 9:
                text = "炊く前の生米を煮て作るイタリア料理。ちなみに、お米は洗わずに使う。ちなみに、ドリアは炊いたご飯にソースをかけて焼いたもの。";
                break;
            case 10:
                text = "炊いたご飯を具材と一緒に炒めた中華料理。ご飯と卵を混ぜてから炒めたり、ご飯を水洗いしたり、パラパラにするコツはいろいろ！";
                break;
            case 11:
                text = "福井県や福島県で食べられる、ウスターソースで味付けしたカツを丼飯にのせた郷土料理。ご飯の上に千切りキャベツを盛ることが多い。";
                break;
            case 12:
                text = "丸く薄くのばした生地に具をのせて窯で焼いた料理。マルゲリータはバジル・チーズ・トマトがイタリア国旗の色を表している。";
                break;
            case 13:
                text = "甘めの生地に、タマネギやチーズ、ベーコンをトッピングしたパン。ベーコンの塩気と甘く焼けた玉ねぎのバランスが絶妙！";
                break;
            case 14:
                text = "豚肉をいれたり、魚介をいれたり。生地と具材を混ぜて焼いたり、生地だけを薄く焼いたり。麺をいれたり……。作り方は”お好み”で。";
                break;
            case 15:
                text = "イタリア生まれの野菜たっぷりのスープ。材料はトマトやじゃがいも、キャベツなどなど。豆や短いパスタをいれることもある。";
                break;
            case 16:
                text = "じゃがいもとベーコンを炒めた、ビールによくあうドイツ感満載の料理。しかし、ドイツにジャーマンポテトという名前の料理はない。";
                break;
            case 17:
                text = "じゃがいもやウインナー、キャベツをごろごろいれた洋風のおでん。ポトフの”ポト”は、フランス語でポット＝鍋という意味。";
                break;
            case 18:
                text = "お肉やサルサ（トマトソース）をトルティーヤ（トウモロコシで作る生地）で包んで食べるメキシコ料理。タコライスの方が有名かな？";
                break;
            case 19:
                text = "卵を焼くだけにみえて、丸くふわふわにするには温度や焼き方がかなりシビア。しあげにケチャップでお絵かきするのも楽しい！";
                break;
            case 20:
                text = "トマトをベースにいろんな魚介のうまみが詰まった南フランスの料理。トムヤムクン、フカヒレスープに並ぶ世界三大スープのひとつ。";
                break;
            case 21:
                text = "ひき肉に刻んだタマネギなどを混ぜて焼いた料理。両手でペチペチしているのは空気を抜いているだけで遊んでいるわけじゃないよ！";
                break;
            case 22:
                text = "鶏がらスープの素で味付けする。どこの家の冷蔵庫にもひそんでる赤いあれ。卵やタマネギ、わかめ、きのこをいれることが多い。";
                break;
            case 23:
                text = "うす切りの生魚にオリーブオイルやソースをかけた前菜の定番。魚を使ったものは日本生まれで、イタリアでは牛肉を使うことが多い。";
                break;
            case 24:
                text = "豚肉とキャベツをテンメンジャンで甘辛く炒めた中華料理。”回鍋”とは一度調理した食材をもう一度鍋に戻して調理するという意味。";
                break;
            case 25:
                text = "関西ではポピュラーな鉄板焼き。炒めた豚肉とキャベツを玉子で包んで作る。卵に片栗粉やとろろを混ぜるときれいに作りやすい。";
                break;
            case 26:
                text = "アジのフライ。添えるのはやっぱりキャベツの千切り！葉の筋に垂直に切ること、切った後に冷たい水にさらすことがポイント！";
                break;
        }


        return text;

    }

    public string[] DishEffectText(int specialID, bool isStrong)
    {
        string[] effectText = { null, null, null };
        //string rareText = null;

        //effectText[2] = "";

        switch (specialID)
        {
            case 0:
                effectText[0] = "ランダムで" + "\n効果が発動する";
                effectText[1] = "発動した効果が強くなる";
                break;
            case 1:
                effectText[0] = "カロリーが" + "\n１になることがある";
                effectText[1] = "１になる確率が下がる";
                break;
            case 2:
                effectText[0] = "どちらかに1人に" + "\n大ダメージを与える";
                effectText[1] = "相手に当たりやすくなる";
                break;
            case 3:
                effectText[0] = "相手のコストが多いほど" + "\n追加ダメージを与える";
                effectText[1] = "追加ダメージアップ";
                break;
            case 4:
                effectText[0] = "自分の状態異常が多いほど" + "\n追加ダメージを与える";
                effectText[1] = "追加ダメージアップ";
                break;
            case 5:
                effectText[0] = "ターンが経過するほど" + "\n追加ダメージを与える";
                effectText[1] = "追加ダメージアップ";
                break;
            case 6:
                effectText[0] = "相手のレア食材を捨てる";
                effectText[1] = "捨てる食材が増える";
                break;
            case 7:
                effectText[0] = "自分の食材をレアにする";
                effectText[1] = "レアにする食材が増える";
                break;
            case 8:
                effectText[0] = "相手のレア食材を奪う";
                effectText[1] = "奪うレア食材が増える";
                break;
            case 9:
                effectText[0] = "HPを回復する";
                effectText[1] = "回復量が増える";
                break;
            case 10:
                effectText[0] = "相手のHPを吸収する";
                effectText[1] = "吸収量が増える";
                break;
            case 11:
                effectText[0] = "自分の状態異常を回復する";
                effectText[1] = "状態異常を相手に移す";
                break;
            case 12:
                effectText[0] = "お互いが料理した時" + "\nカロリーが増加する";
                effectText[1] = "カロリー増加量アップ";
                break;
            case 13:
                effectText[0] = "相手が状態異常の時" + "\nカロリーが増加する";
                effectText[1] = "カロリー増加量アップ";
                break;
            case 14:
                effectText[0] = "相手が悪い食材を持っている時" + "\nカロリーが増加する";
                effectText[1] = "カロリー増加量アップ";
                break;
            case 15:
                effectText[0] = "相手の料理コストを奪う";
                effectText[1] = "奪うコスト量アップ";
                break;
            case 16:
                effectText[0] = "自分の料理コストを増やす";
                effectText[1] = "コスト増加量アップ";
                break;
            case 17:
                effectText[0] = "相手の料理コストを減らす";
                effectText[1] = "コスト減少量アップ";
                break;
            case 18:
                effectText[0] = "相手を不器用状態にする";
                effectText[1] = "状態異常ターンが増える";
                effectText[2] = "不器用：選択時間が短くなる";
                break;
            case 19:
                effectText[0] = "相手を暗闇状態にする";
                effectText[1] = "状態異常ターンが増える";
                effectText[2] = "暗闇：料理結果が見えない";
                break;
            case 20:
                effectText[0] = "相手を毒状態にする";
                effectText[1] = "状態異常ターンが増える";
                effectText[2] = "毒：毎ターンダメージ";
                break;
            case 21:
                effectText[0] = "このターン受ける" + "\nダメージが減少する";
                effectText[1] = "ダメージ減少量アップ";                
                break;
            case 22:
                effectText[0] = "このターン受けた" + "\nダメージを相手に返す";
                effectText[1] = "返すダメージ量アップ";
                break;
            case 23:
                effectText[0] = "このターン相手の" + "\n料理効果を無効にする";
                effectText[1] = "食材の効果も無効にする";
                break;
            case 24:
                effectText[0] = "相手の赤食材を悪くする";
                effectText[1] = "悪くする食材が増える";
                effectText[2] = "悪い食材：料理に使えない";
                break;
            case 25:
                effectText[0] = "相手の黄食材を悪くする";
                effectText[1] = "悪くする食材が増える";
                effectText[2] = "悪い食材：料理に使えない";
                break;
            case 26:
                effectText[0] = "相手の緑食材を悪くする";
                effectText[1] = "悪くする食材が増える";
                effectText[2] = "悪い食材：料理に使えない";
                break;
            default:
                effectText[0] = "エラー";
                break;


        }

        if (isStrong)
        {
            effectText[1] = "\nレア：" + effectText[1];
        }
        else
        {
            effectText[1] = "";
        }

        if (effectText[2] == null)
        {
            effectText[2] = "";
        }
        else
        {
            effectText[2] = "\n" + effectText[2];
        }

        return effectText;
    }

}
