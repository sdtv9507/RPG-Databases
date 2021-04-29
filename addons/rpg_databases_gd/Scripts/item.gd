extends Container
tool

# Declare member variables here. Examples:
# var a = 2
# var b = "text"

var icon_path: String = ""
var item_selected: int = 0
# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.

func start() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Item")
	$ItemButton.clear()
	for i in range(json_dictionary.size()):
		var item_data: Dictionary = json_dictionary["item"+String(i)]
		if i > $ItemButton.get_item_count() - 1:
			$ItemButton.add_item(item_data["name"])
		else:
			$ItemButton.set_item_text(i, item_data["name"])
	json_dictionary = get_parent().get_parent().call("read_data", "System")
	var system_data: Dictionary = json_dictionary["elements"]
	for i in range(system_data.size()):
		if i >$DamageLabel/ElementLabel/ElementButton.get_item_count() - 1:
			$DamageLabel/ElementLabel/ElementButton.add_item(system_data[String(i)])
		else:
			$DamageLabel/ElementLabel/ElementButton.set_item_text(i, system_data[String(i)])
	refresh_data(item_selected)

func refresh_data(id: int) -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Item")
	var item_data: Dictionary = json_dictionary["item"+String(id)]
	$NameLabel/NameText.text = item_data["name"]
	var icon: String = item_data["icon"]
	if icon != "":
		icon_path = icon
		$IconLabel/IconSprite.texture = load(item_data["icon"])
	$DescLabel/DescText.text = item_data["description"]
	$ItemTypeLabel/ItemTypeButton.selected = item_data["item_type"]
	$PriceLabel/PriceBox.value = item_data["price"]
	$ConsumableLabel/ConsumableButton.selected = item_data["consumable"]
	$TargetLabel/TargetButton.selected = item_data["target"]
	$UsableLabel/UsableButton.selected = item_data["usable"]
	$HitLabel/HitBox.value = item_data["success"]
	$TypeLabel/TypeButton.selected = item_data["hit_type"]
	$DamageLabel/DTypeLabel/DTypeButton.selected = item_data["damage_type"]
	$DamageLabel/ElementLabel/ElementButton.selected = item_data["element"]
	$DamageLabel/DFormulaLabel/FormulaText.text = item_data["formula"]
	if item_data.has("effects"):
		clear_effect_list()
		var effect_list: Array = item_data["effects"]
		for effect in effect_list:
			add_effect_list(effect["name"], effect["data_id"], effect["value1"], effect["value2"])

func _on_Search_pressed() -> void:
	$IconLabel/IconSearch.popup_centered()

func _on_IconSearch_file_selected(path: String) -> void:
	icon_path = path
	$IconLabel/IconSprite.texture = load(path)

func _on_AddItem_pressed() -> void:
	$ItemButton.add_item("NewItem")
	var id: int = $ItemButton.get_item_count() - 1
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Item")
	var item_data: Dictionary
	item_data["name"] = "NewItem"
	item_data["icon"] = ""
	item_data["description"] = "New created item"
	item_data["item_type"] = 0
	item_data["price"] = 10
	item_data["consumable"] = 0
	item_data["target"] = 3
	item_data["usable"] = 0
	item_data["success"] = 95
	item_data["hit_type"] = 1
	item_data["damage_type"] = 1
	item_data["element"] = 0
	item_data["formula"] = "10"
	json_dictionary["item" + String(id)] = item_data
	get_parent().get_parent().call("store_data", "Item", json_dictionary)

func _on_RemoveItemButton_pressed():
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Item")
	if json_dictionary.keys().size() > 1:
		var item_id: int = item_selected
		while item_id < (json_dictionary.keys().size() - 1):
			json_dictionary["item"+String(item_id)] = json_dictionary["item"+String(item_id+1)]
			item_id += 1
		json_dictionary.erase("item"+String(item_id))
		get_parent().get_parent().call("store_data", "Item", json_dictionary)
		$CharacterButton.remove_item(item_selected)
		if item_selected == 0:
			$CharacterButton.select(item_selected + 1)
			item_selected += 1
		else:
			$CharacterButton.select(item_selected + 1)
			item_selected -= 1
		$CharacterButton.select(item_selected)
		refresh_data(item_selected)

func _on_ItemSaveButton_pressed() -> void:
	save_item_data()
	refresh_data(item_selected)

func save_item_data() -> void:
	var json_dictionary: Dictionary = get_parent().get_parent().call("read_data", "Item")
	var item_data: Dictionary = json_dictionary["item"+String(item_selected)]
	var effect_list: Array
	item_data["name"] = $NameLabel/NameText.text
	$ItemButton.set_item_text(item_selected, $NameLabel/NameText.text)
	item_data["icon"] = icon_path
	item_data["description"] = $DescLabel/DescText.text
	item_data["item_type"] = $ItemTypeLabel/ItemTypeButton.selected
	item_data["price"] = $PriceLabel/PriceBox.value
	item_data["consumable"] = $ConsumableLabel/ConsumableButton.selected
	item_data["target"] = $TargetLabel/TargetButton.selected
	item_data["usable"] = $UsableLabel/UsableButton.selected
	item_data["success"] = $HitLabel/HitBox.value
	item_data["hit_type"] = $TypeLabel/TypeButton.selected
	item_data["damage_type"] = $DamageLabel/DTypeLabel/DTypeButton.selected
	item_data["element"] = $DamageLabel/ElementLabel/ElementButton.selected
	item_data["formula"] = $DamageLabel/DFormulaLabel/FormulaText.text
	var effect_size: int = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_count()
	for i in effect_size:
		var effect_data: Dictionary
		effect_data["name"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_item_text(i)
		effect_data["data_id"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.get_item_text(i)
		effect_data["value1"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.get_item_text(i)
		effect_data["value2"] = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.get_item_text(i)
		effect_list.append(effect_data)
	item_data["effects"] = effect_list
	get_parent().get_parent().call("store_data", "Item", json_dictionary)

func _on_ItemButton_item_selected(id: int) -> void:
	item_selected = id
	refresh_data(id)

func _on_AddItemEffect_pressed() -> void:
	get_parent().get_parent().call("open_effect_manager", "Item")

func _on_RemoveItemEffect_pressed() -> void:
	var id: int = $EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.get_selected_items()[0]
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1.remove_item(id)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2.remove_item(id)

func add_effect_list(name: String, data_id: int, value1: String, value2: String) -> void:
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames.add_item(name)
	$EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType.add_item(data_id)
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
