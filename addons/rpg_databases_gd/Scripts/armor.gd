extends Container
tool

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var icon_path: String = ""
var armor_selected: int = 0
var stat_edit: int = -1
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Armor")
	$ArmorButton.clear()
	for i in range(json_dictionary.size()):
		var armor_data: Dictionary = json_dictionary["armor"+String(i)]
		if i > $ArmorButton.get_item_count() - 1:
			$ArmorButton.add_item(armor_data["name"])
		else:
			$ArmorButton.set_item_text(i, armor_data["name"])
	json_dictionary = get_parent().get_parent().call("read_data", "System")
	var system_data: Dictionary = json_dictionary["armors"]
	for i in range(system_data.size()):
		if i > $ATypeLabel/ATypeButton.get_item_count() - 1:
			$ATypeLabel/ATypeButton.add_item(system_data[String(i)])
		else:
			$ATypeLabel/ATypeButton.set_item_text(i, system_data[String(i)])
	system_data = json_dictionary["slots"]
	var final_id: int = 0
	for string in system_data.keys():
		if string[0] == "w":
			var id = string
			id.erase(0, 1)
			if int(id) > $SlotLabel/SlotButton.get_item_count() - 1:
				$SlotLabel/SlotButton.add_item(system_data[string])
			else:
				$SlotLabel/SlotButton.set_item_text(int(id), system_data[string])
		else:
			final_id += 1
	refresh_data(armor_selected)

func refresh_data(id: int) -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Armor")
	var armor_data: Dictionary = json_dictionary["armor" + String(id)]
	json_dictionary = get_parent().get_parent().call("read_data", "System")
	var system_data: Dictionary = json_dictionary["stats"]
	$NameLabel/NameText.text = armor_data["name"]
	var icon: String = armor_data["icon"]
	if icon != "":
		icon_path = icon
		$IconLabel/IconSprite.texture = load(icon)
	$DescLabel/DescText.text = armor_data["description"]
	$ATypeLabel/ATypeButton.selected = int(armor_data["armor_type"])
	$SlotLabel/SlotButton.selected = int(armor_data["slot_type"])
	$PriceLabel/PriceSpin.value = int(armor_data["price"])
	
	$StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList.clear()
	$StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList.clear()
	for i in range(system_data.size()):
		var stat_name: String = system_data[String(i)]
		var armor_stat_formula: Dictionary = armor_data["stat_list"]
		var stat_formula: String = ""
		if armor_stat_formula.has(stat_name):
			stat_formula = armor_stat_formula[stat_name]
		else:
			stat_formula = "0"
		$StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList.add_item(stat_name)
		$StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList.add_item(stat_formula)
	if armor_data.has("effects") == true:
		clear_effect_list()
		var effect_list: Array = armor_data["effects"]
		for effect in effect_list:
			add_effect_list(effect["name"], int(effect["data_id"]), String(effect["value1"]), String(effect["value2"]))

func _on_AddArmorButton_pressed() -> void:
	$ArmorButton.add_item("NewArmor")
	var id: int = $ArmorButton.get_item_count() - 1
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Armor")
	var armor_data: Dictionary
	var armor_stats: Dictionary
	armor_data["name"] = "NewArmor"
	armor_data["icon"] = ""
	armor_data["description"] = "New created armor"
	armor_data["armor_type"] = 0
	armor_data["slot_type"] = 0
	armor_data["price"] = 50
	armor_stats["hp"] = "0"
	armor_stats["mp"] = "0"
	armor_stats["atk"] = "0"
	armor_stats["def"] = "0"
	armor_stats["int"] = "0"
	armor_stats["res"] = "0"
	armor_stats["spd"] = "0"
	armor_stats["luk"] = "0"
	armor_data["stat_list"] = armor_stats
	json_dictionary["armor" + String(id)] = armor_data
	get_parent().get_parent().call("store_data", "Armor", json_dictionary)

func _on_RemoveArmor_pressed() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Armor")
	if json_dictionary.keys().size() > 1:
		var armor_id: int = armor_selected
		while armor_id < (json_dictionary.keys().size() - 1):
			json_dictionary["armor"+String(armor_id)] = json_dictionary["armor"+String(armor_id+1)]
			armor_id += 1
		json_dictionary.erase("armor"+String(armor_id))
		get_parent().get_parent().call("store_data", "Armor", json_dictionary)
		$ArmorButton.remove_item(armor_selected)
		if armor_selected == 0:
			$ArmorButton.select(armor_selected + 1)
			armor_selected += 1
		else:
			$ArmorButton.select(armor_selected + 1)
			armor_selected -= 1
		$ArmorButton.select(armor_selected)
		refresh_data(armor_selected)

func _on_ArmorSaveButton_pressed() -> void:
	save_armor_data()
	refresh_data(armor_selected)

func _on_Search_pressed() -> void:
	$IconLabel/IconSearch.popup_centered()

func _on_IconSearch_file_selected(path: String) -> void:
	icon_path = path
	$IconLabel/IconSprite.texture = load(path)

func _on_ArmorButton_item_selected(id: int) -> void:
	armor_selected = id
	refresh_data(id)

func save_armor_data() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Armor")
	var armor_data: Dictionary = json_dictionary["armor"+String(armor_selected)]
	var armor_stat_formula: Dictionary = armor_data["stat_list"]
	var effect_list: Array
	armor_data["name"] = $NameLabel/NameText.text
	$ArmorButton.set_item_text(armor_selected, $NameLabel/NameText.text)
	armor_data["icon"] = icon_path
	armor_data["description"] = $DescLabel/DescText.text
	armor_data["armor_type"] = $ATypeLabel/ATypeButton.selected
	armor_data["slot_type"] = $SlotLabel/SlotButton.selected
	armor_data["price"] = $PriceLabel/PriceSpin.value
	var items: int = $StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList.get_item_count()
	for i in range(items):
		var stat: String = $StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList.get_item_text(i)
		var formula: String = $StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList.get_item_text(i)
		armor_stat_formula[stat] = formula
	var effect_size: int = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_count()
	for i in effect_size:
		var effect_data: Dictionary
		effect_data["name"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_text(i)
		effect_data["data_id"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.get_item_text(i)
		effect_data["value1"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.get_item_text(i)
		effect_data["value2"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.get_item_text(i)
		effect_list.append(effect_data)
	armor_data["effects"] = effect_list
	get_parent().get_parent().call("store_data", "Armor", json_dictionary)

func _on_StatValueList_item_activated(index: int) -> void:
	var stat_name: String = $StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList.get_item_text(index)
	var stat_formula: String = $StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList.get_item_text(index)
	$StatEditor/StatLabel.text = stat_name
	$StatEditor/StatEdit.text = stat_formula
	stat_edit = index
	$StatEditor.show()

func _on_OkButton_pressed() -> void:
	var stat_formula: String = $StatEditor/StatEdit.text
	$StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList.set_item_text(stat_edit, stat_formula)
	save_armor_data()
	stat_edit = -1
	$StatEditor.hide()

func _on_CancelButton_pressed() -> void:
	stat_edit = -1
	$StatEditor.hide()

func  _on_AddArmorEffect_pressed() -> void:
	get_parent().get_parent().call("open_effect_manager", "Armor")

func _on_RemoveArmorEffect_pressed() -> void:
	var id: int = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_selected_items()[0]
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.remove_item(id)

func add_effect_list(name: String, data_id: int, value1: String, value2: String) -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.add_item(name)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.add_item(String(data_id))
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.add_item(value1)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.add_item(value2)

func clear_effect_list() -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.clear()
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.clear()

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
