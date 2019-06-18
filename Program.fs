open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Writers
open Newtonsoft.Json

let JSON v =
  let settings = JsonSerializerSettings()
  JsonConvert.SerializeObject(v, settings)

let SetJsonHeader =
  setMimeType "application/json; charset=utf-8"

type Company = {
  code: string
  name: string
  address: string
}

let companies =
  seq {
    yield { code = "123"; name = "テレビ東京"; address = "東京都港区六本木三丁目2番1号 住友不動産六本木グランドタワー内"  }
    yield { code = "456"; name = "テレビ朝日"; address = "東京都港区六本木六丁目9番1号" }
    yield { code = "789"; name = "日本テレビ"; address = "東京都港区東新橋一丁目6-1" }
  }
  |> Seq.toList
  
  
let GetCompanies() =
  companies |> JSON


let GetCompany code =
  List.find(fun c -> c.code = code) |> JSON


let webApp =
  choose [
    path "/companies" >=> OK(GetCompanies()) >=> SetJsonHeader
    pathScan "/companies/%s" (fun code -> OK(GetCompany code)) >=> SetJsonHeader
]

startWebServer defaultConfig webApp


